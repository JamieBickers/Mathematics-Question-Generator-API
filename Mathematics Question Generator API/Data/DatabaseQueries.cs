using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class DatabaseQueries
    {
        private readonly QuestionGeneratorContext context;

        public DatabaseQueries(QuestionGeneratorContext context)
        {
            this.context = context;
        }

        public void InsertUser(string emailAddress)
        {
            if (!EmailAddress.IsEmailAddressValid(emailAddress))
            {
                throw new ArgumentException("Invalid email address.");
            }
            else if (!context.UserDbo.Any(user => user.EmailAddress == emailAddress))
            {
                context.UserDbo.Add(new UserDbo(emailAddress));
                context.SaveChanges();
            }
        }

        public bool CheckIfUserIsInDatabase(string emailAddress)
        {
            return context.UserDbo.Any(user => user.EmailAddress == emailAddress);
        }

        public List<List<IQuestion>> SelectAllWorksheetsByUser(string userEmailAddress)
        {
            if (!EmailAddress.IsEmailAddressValid(userEmailAddress))
            {
                throw new ArgumentException("Invalid email address.");
            }

            return context.WorksheetDbo
                .Include(worksheet => worksheet.User)
                .Where(worksheetDbo => worksheetDbo.User.EmailAddress == userEmailAddress)
                .Select(SelectWorksheetQuestions)
                .ToList();
        }

        private List<IQuestion> SelectWorksheetQuestions(WorksheetDbo worksheetDbo)
        {
            return context.WorksheetQuestionDbo
                .Include(worksheetQuestion => worksheetQuestion.Question)
                .Include(worksheetQuestion => worksheetQuestion.Question.QuestionType)
                .Include(worksheetQuestion => worksheetQuestion.Worksheet.User)
                .Where(worksheetQuestion => worksheetQuestion.Worksheet.ID == worksheetDbo.ID)
                .ToList()
                .OrderBy(worksheetQuestion => worksheetQuestion.QuestionNumber)
                .Select(worksheetQuestion => FindQuestionCorrespondingToQuestionDbo(worksheetQuestion.Question))
                .ToList();
        }

        private IQuestion FindQuestionCorrespondingToQuestionDbo(QuestionDbo question)
        {
            var questionType = question.QuestionType.Type;

            if (questionType == "quadratic")
            {
                return  context.QuadraticEquationDbo
                    .First(quadratic => quadratic.ID == question.SpecificQuestionID)
                    .CovertToQuadraticEquation();
            }
            else if (questionType == "simultaneous")
            {
                return context.LinearSimultaneousEquationsDbo
                    .First(simultaneous => simultaneous.ID == question.SpecificQuestionID)
                    .CovertToLinearSimultaneousEquations();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void InsertWorksheet(List<IQuestion> questions, string userEmailAddress)
        {
            if (!EmailAddress.IsEmailAddressValid(userEmailAddress))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (!CheckIfUserIsInDatabase(userEmailAddress))
            {
                InsertUser(userEmailAddress);
                context.SaveChanges();
            }

            var currentUser = context.UserDbo.First(user => user.EmailAddress == userEmailAddress);
            var worksheet = new WorksheetDbo() { User = currentUser, DateSent = DateTimeOffset.Now };

            context.WorksheetDbo.Add(worksheet);

            var questionDbos = questions.Select(question => InsertQuestion(question))
                .ToList();

            for (var i = 0; i < questionDbos.Count(); i++)
            {
                context.WorksheetQuestionDbo.Add(new WorksheetQuestionDbo(questionDbos[i], worksheet, i));
            }
            context.SaveChanges();
        }

        private QuestionDbo InsertQuestion(IQuestion question)
        {
            if (question is QuadraticEquation quadratic)
            {
                var quadraticDbo = new QuadraticEquationDbo(quadratic);
                return AddQuadratic(quadraticDbo);
            }
            else if (question is LinearSimultaneousEquations simultaneous)
            {
                var simultaneousDbo = new LinearSimultaneousEquationsDbo(simultaneous);
                return AddSimultaneous(simultaneousDbo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private QuestionDbo AddQuadratic(QuadraticEquationDbo equation)
        {
            var questionType = context.QuestionTypeDbo.First(type => type.Type == "quadratic");
            context.QuadraticEquationDbo.Add(equation);
            context.SaveChanges();

            var question = new QuestionDbo(equation.ID, questionType);
            context.QuestionDbo.Add(question);
            context.SaveChanges();

            return question;
        }

        private QuestionDbo AddSimultaneous(LinearSimultaneousEquationsDbo equations)
        {
            var questionType = context.QuestionTypeDbo.First(type => type.Type == "simultaneous");
            context.LinearSimultaneousEquationsDbo.Add(equations);
            context.SaveChanges();

            var question = new QuestionDbo(equations.ID, questionType);
            context.QuestionDbo.Add(question);
            context.SaveChanges();

            return question;
        }
    }
}