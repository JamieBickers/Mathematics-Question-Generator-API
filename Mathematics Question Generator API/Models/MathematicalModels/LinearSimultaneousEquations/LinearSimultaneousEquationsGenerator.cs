using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGenerator
        : QuestionGenerator<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters,List<LinearEquation>, LinearSimultaneousEquationsSolution>
    {
        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }

        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator, LinearSimultaneousEquationsGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        protected override LinearSimultaneousEquationsSolution CalculateSolution(List<LinearEquation> coefficients, out bool invalidCoefficients)
        {
            return LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients, out invalidCoefficients);
        }

        protected override bool CheckValidQuestion(List<LinearEquation> coefficients, LinearSimultaneousEquationsSolution solution)
        {
            if ((coefficients[0].XTerm == 0 && coefficients[0].YTerm == 0) || (coefficients[1].XTerm == 0 && coefficients[1].YTerm == 0))
            {
                return false;
            }
            else if (!solution.InfiniteSolutions && parameters.RequireInfiniteSolutions)
            {
                return false;
            }
            else if (!solution.NoSolution && parameters.RequireNoSolutions)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override List<LinearEquation> GenerateRandomCoefficients()
        {
            var randomNumbers = Enumerable.Range(0, 6)
                .Select(x => randomIntegerGenerator.GenerateRandomInteger(parameters.CoefficientLowerBound, parameters.CoefficientUpperBound)).ToList();

            var firstEquation = new LinearEquation(randomNumbers[0], randomNumbers[1], randomNumbers[2]);
            var secondEquation = new LinearEquation(randomNumbers[3], randomNumbers[4], randomNumbers[5]);

            return new List<LinearEquation>() { firstEquation, secondEquation };
        }

        protected override Func<List<LinearEquation>, LinearSimultaneousEquationsSolution, LinearSimultaneousEquations> ComputeContructorForQuestion()
        {
            return (coefficients, solutions) => new LinearSimultaneousEquations(coefficients, solutions);
        }

        protected override Func<LinearSimultaneousEquationsGeneratorParameters> ComputeContructorForParameters()
        {
            return () => new LinearSimultaneousEquationsGeneratorParameters();
        }
    }
}
