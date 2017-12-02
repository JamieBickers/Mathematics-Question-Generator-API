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

            //var integerGenerator = new FixedRandomIntegerGenerator(123);

            //var users = new List<UserDbo>();

            //var worksheets = new List<WorksheetDbo>();

            var questionTypes = new List<QuestionTypeDbo>()
            {
                new QuestionTypeDbo() { Type = "quadratic"},
                new QuestionTypeDbo() { Type = "simultaneous"}
            };

            //var quadraticEquations = new List<QuadraticEquationDbo>();

            //var simultaneousEquations = new List<LinearSimultaneousEquationsDbo>();

            //var quadraticQuestions = quadraticEquations
            //    .Select(equation => new QuestionDbo(equation.ID, questionTypes[0]))
            //    .ToList();

            //var simultaneousQuestions = simultaneousEquations
            //    .Select(equation => new QuestionDbo(equation.ID, questionTypes[1]))
            //    .ToList();

            //var questions = quadraticQuestions
            //    .Select((quadratic, index) => new QuestionDbo(index, questionTypes[0]))
            //    .ToList();

            //questions.AddRange(simultaneousEquations
            //.Select((equation, index) => new QuestionDbo(index, questionTypes[1])));

            //var worksheetQuestions =
            //    Enumerable.Range(0, 50)
            //    .Select(x => new WorksheetQuestionDbo(questions[integerGenerator.GenerateRandomInteger(200)], worksheets[integerGenerator.GenerateRandomInteger(5)], x))
            //    .ToList();

            //users.ForEach(user => context.UserDbo.Add(user));
            //worksheets.ForEach(worksheet => context.WorksheetDbo.Add(worksheet));
            //quadraticEquations.ForEach(quadratic => context.QuadraticEquationDbo.Add(quadratic));
            questionTypes.ForEach(questionType => context.QuestionTypeDbo.Add(questionType));
            //simultaneousEquations.ForEach(equation => context.LinearSimultaneousEquationsDbo.Add(equation));
            //questions.ForEach(question => context.QuestionDbo.Add(question));
            //worksheetQuestions.ForEach(worksheetQuestion => context.WorksheetQuestionDbo.Add(worksheetQuestion));

            context.SaveChanges();
        }
    }
}
