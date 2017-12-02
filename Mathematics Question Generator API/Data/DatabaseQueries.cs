using MathematicsQuestionGeneratorAPI.Models;
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
            if (!context.UserDbo.Any(user => user.EmailAddress == emailAddress))
            {
                context.UserDbo.Add(new Models.UserDbo(emailAddress));
                context.SaveChanges();
            }
        }

        public void InsertQuadraticWorksheet(List<QuadraticEquation> equations, string userEmailAddress)
        {
            Func<QuadraticEquation, QuadraticEquationDbo> dboConstructor =
                quadratic => new QuadraticEquationDbo(quadratic);

            InsertWorksheet(equations, userEmailAddress, dboConstructor, AddQuadratic);
        }

        public void InsertSimultaneousWorksheet(List<LinearSimultaneousEquations> equations, string userEmailAddress)
        {
            Func<LinearSimultaneousEquations, LinearSimultaneousEquationsDbo> dboConstructor =
                linearEquations => new LinearSimultaneousEquationsDbo(linearEquations);

            InsertWorksheet(equations, userEmailAddress, dboConstructor, AddSimultaneous);
        }

        public List<List<IQuestion>> SelectAllWorksheetsByUser(string userEmailAddress)
        {
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

        private void InsertWorksheet<TQuestion, TQuestionDbo>(List<TQuestion> equations, string userEmailAddress,
            Func<TQuestion, TQuestionDbo> dboConstructor, Func<TQuestionDbo, QuestionDbo> addSpecificQuestion)
        {
            if (!context.UserDbo.Any(user => user.EmailAddress == userEmailAddress))
            {
                InsertUser(userEmailAddress);
                context.SaveChanges();
            }
            var currentUser = context.UserDbo.First(user => user.EmailAddress == userEmailAddress);
            var worksheet = new WorksheetDbo() { User = currentUser, DateSent = DateTimeOffset.Now };

            var equationDbos = equations.Select(question => dboConstructor(question));
            var questions = equationDbos.Select(equation => addSpecificQuestion(equation)).ToList();

            for (var i = 0; i < questions.Count; i++)
            {
                context.WorksheetQuestionDbo.Add(new WorksheetQuestionDbo(questions[i], worksheet, i));
            }
            context.SaveChanges();
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