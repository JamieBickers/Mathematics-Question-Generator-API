using MathematicsQuestionGeneratorAPI.Exceptions;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels
{
    public abstract class QuestionGenerator<TQuestion, TParameters, TCoefficients, TSolution> : IQuestionGenerator<TQuestion>
        where TParameters : IValidatableObject
        where TQuestion : IQuestion
    {
        protected TParameters parameters;
        protected readonly IRandomIntegerGenerator randomIntegerGenerator;
        protected const int MaxNumberOfTries = 1000000;
        protected Func<TCoefficients, TSolution, TQuestion> ConstructorForQuestion;
        protected Func<TParameters> ContructorForParameters;

        public QuestionGenerator(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            ConstructorForQuestion = ComputeContructorForQuestion();
            ContructorForParameters = ComputeContructorForParameters();
            parameters = ContructorForParameters();
        }

        public QuestionGenerator(IRandomIntegerGenerator randomIntegerGenerator, TParameters parameters)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.parameters = parameters;
            ConstructorForQuestion = ComputeContructorForQuestion();
            ContructorForParameters = ComputeContructorForParameters();
        }

        public TQuestion GenerateQuestionAndAnswer()
        {
            TSolution solution;
            var coefficients = GenerateValidCoefficients(out solution);
            var question = ConstructorForQuestion(coefficients, solution);
            return question;
        }

        // If the coefficients are invalid, set invalidCoefficients to true and return a stub.
        protected abstract TSolution CalculateSolution(TCoefficients coefficients, out bool invalidCoefficients);

        protected virtual TCoefficients GenerateValidCoefficients(out TSolution solution)
        {
            TCoefficients coefficients;
            bool invalidCoefficients = false;

            var numberOfTries = 0;

            do
            {
                if (numberOfTries > MaxNumberOfTries)
                {
                    throw new FailedToGenerateQuestionSatisfyingParametersException();
                }
                coefficients = GenerateRandomCoefficients();
                solution = CalculateSolution(coefficients, out invalidCoefficients);
                numberOfTries++;
                if (invalidCoefficients)
                {
                    continue;
                }
            } while (!CheckValidQuestion(coefficients, solution));

            return coefficients;
        }

        protected abstract bool CheckValidQuestion(TCoefficients coefficients, TSolution solution);

        protected abstract TCoefficients GenerateRandomCoefficients();

        protected abstract Func<TCoefficients, TSolution, TQuestion> ComputeContructorForQuestion();
        protected abstract Func<TParameters> ComputeContructorForParameters();
    }
}
