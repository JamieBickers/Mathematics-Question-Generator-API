using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels
{
    public abstract class QuestionGenerator<TQuestion, TParameters, TCoefficient, TSolution, TCoefficients, TSolutions> : IQuestionGenerator<TQuestion>
        where TParameters : QuestionParameters
        where TCoefficients : IEnumerable<TCoefficient>
        where TSolutions : IEnumerable<TSolution>
        where TQuestion : IQuestion
    {
        protected TParameters parameters;
        protected readonly IRandomIntegerGenerator randomIntegerGenerator;
        protected const int MaxNumberOfTries = 1000000;
        protected Func<TCoefficients, TSolutions, TQuestion> ConstructorForQuestion;
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
            TCoefficients coefficients = GenerateValidCoefficients();
            TSolutions solutions = CalculateSolutions(coefficients);
            TQuestion question = ConstructorForQuestion(coefficients, solutions);
            return question;
        }

        protected abstract TSolutions CalculateSolutions(TCoefficients coefficients);

        protected virtual TCoefficients GenerateValidCoefficients()
        {
            TCoefficients coefficients;

            var numberOfTries = 0;

            do
            {
                if (numberOfTries > MaxNumberOfTries)
                {
                    throw new Exception("Could not generate question satisfying conditions.");
                }
                coefficients = GenerateRandomCoefficients();
                numberOfTries++;
            } while (!CheckValidCoefficients(coefficients));

            return coefficients;
        }

        protected abstract bool CheckValidCoefficients(TCoefficients coefficients);

        protected abstract TCoefficients GenerateRandomCoefficients();

        protected abstract Func<TCoefficients, TSolutions, TQuestion> ComputeContructorForQuestion();
        protected abstract Func<TParameters> ComputeContructorForParameters();
    }
}
