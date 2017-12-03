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

            var questionTypes = new List<QuestionTypeDbo>()
            {
                new QuestionTypeDbo() { Type = "quadratic"},
                new QuestionTypeDbo() { Type = "simultaneous"}
            };

            questionTypes.ForEach(questionType => context.QuestionTypeDbo.Add(questionType));

            context.SaveChanges();
        }
    }
}
