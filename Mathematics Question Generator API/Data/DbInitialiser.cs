using MathematicsQuestionGeneratorAPI.DBOs;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(QuestionGeneratorContext context)
        {
            context.Database.EnsureCreated();

            if (context.UserDbo.Any())
            {
                return;
            }

            var integerGenerator = new FixedRandomIntegerGenerator(123);

            var users = new List<UserDbo>()
            {
                //new UserDbo("bickersjamie@googlemail.com")
            };

            var worksheets = new List<WorksheetDbo>()
            {
                //new WorksheetDbo() { User = users[0], DateSent = DateTimeOffset.Parse("2017/01/01 00:00:00")},
                //new WorksheetDbo() { User = users[0], DateSent = DateTimeOffset.Parse("2017/02/16 13:30:00")},
                //new WorksheetDbo() { User = users[0], DateSent = DateTimeOffset.Parse("2017/01/01 14:38:00")},
                //new WorksheetDbo() { User = users[0], DateSent = DateTimeOffset.Parse("2017/07/09 7:30:09")},
                //new WorksheetDbo() { User = users[0], DateSent = DateTimeOffset.Parse("2017/11/29 13:30:00")}
            };

            var questionTypes = new List<QuestionTypeDbo>()
            {
                new QuestionTypeDbo() { Type = "quadratic"},
                new QuestionTypeDbo() { Type = "simultaneous"}
            };

            var quadraticEquationGenerator = new QuadraticEquationGenerator(integerGenerator);
            var quadraticEquations = new List<QuadraticEquationDbo>();
                //Enumerable.Range(0, 100)
                //.Select(x => new QuadraticEquationDbo(quadraticEquationGenerator.GenerateQuestionAndAnswer()))
                //.ToList();

            var simultaneousEquationsGenerator = new LinearSimultaneousEquationsGenerator(integerGenerator);
            var simultaneousEquations = new List<LinearSimultaneousEquationsDbo>();
            //    Enumerable.Range(0, 100)
            //    .Select(x => new LinearSimultaneousEquationsDbo(simultaneousEquationsGenerator.GenerateQuestionAndAnswer()))
            //    .ToList();

            var quadraticQuestions = quadraticEquations
                .Select(equation => new QuestionDbo(equation.ID, questionTypes[0]))
                .ToList();

            var simultaneousQuestions = simultaneousEquations
                .Select(equation => new QuestionDbo(equation.ID, questionTypes[1]))
                .ToList();

            var questions = quadraticQuestions
                .Select((quadratic, index) => new QuestionDbo(index, questionTypes[0]))
                .ToList();

            questions.AddRange(simultaneousEquations
            .Select((equation, index) => new QuestionDbo(index, questionTypes[1])));

            //var worksheetQuestions =
            //    Enumerable.Range(0, 50)
            //    .Select(x => new WorksheetQuestionDbo(questions[integerGenerator.GenerateRandomInteger(200)], worksheets[integerGenerator.GenerateRandomInteger(5)], x))
            //    .ToList();

            users.ForEach(user => context.UserDbo.Add(user));
            worksheets.ForEach(worksheet => context.WorksheetDbo.Add(worksheet));
            quadraticEquations.ForEach(quadratic => context.QuadraticEquationDbo.Add(quadratic));
            questionTypes.ForEach(questionType => context.QuestionTypeDbo.Add(questionType));
            simultaneousEquations.ForEach(equation => context.LinearSimultaneousEquationsDbo.Add(equation));
            questions.ForEach(question => context.QuestionDbo.Add(question));
            //worksheetQuestions.ForEach(worksheetQuestion => context.WorksheetQuestionDbo.Add(worksheetQuestion));

            context.SaveChanges();
        }
    }
}
