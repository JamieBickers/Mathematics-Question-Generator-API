using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGenerator
        : QuestionGenerator<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters,List<int>, LinearSimultaneousEquationsSolution>
    {
        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }

        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator, LinearSimultaneousEquationsGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        protected override LinearSimultaneousEquationsSolution CalculateSolutions(List<int> coefficients)
        {
            return LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients);
        }

        protected override bool CheckValidCoefficients(List<int> coefficients)
        {
            if ((coefficients[0] == 0 && coefficients[1] == 0) || (coefficients[3] == 0 && coefficients[4] == 0))
            {
                return false;
            }

            var solution = LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients);

            if (!solution.InfiniteSolutions && parameters.RequireInfiniteSolutions)
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

        protected override List<int> GenerateRandomCoefficients()
        {
            return Enumerable.Range(0, 6)
                .Select(x => randomIntegerGenerator.GenerateRandomInteger(parameters.CoefficientLowerBound, parameters.CoefficientUpperBound)).ToList();
        }

        protected override Func<List<int>, LinearSimultaneousEquationsSolution, LinearSimultaneousEquations> ComputeContructorForQuestion()
        {
            return (coefficients, solutions) => new LinearSimultaneousEquations(coefficients, solutions);
        }

        protected override Func<LinearSimultaneousEquationsGeneratorParameters> ComputeContructorForParameters()
        {
            return () => new LinearSimultaneousEquationsGeneratorParameters();
        }
    }
}
