using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGenerator
        : QuestionGenerator<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters, int,
            LinearSimultaneousEquationsSolution, List<int>, List<LinearSimultaneousEquationsSolution>>
    {
        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }

        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator, LinearSimultaneousEquationsGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        protected override List<LinearSimultaneousEquationsSolution> CalculateSolutions(List<int> coefficients)
        {
            var solver = new LinearSimultaneousEquationsAnalysisFunctions();
            return new List<LinearSimultaneousEquationsSolution>() { solver.CalculateSolution(coefficients) };
        }

        protected override bool CheckValidCoefficients(List<int> coefficients)
        {
            if ((coefficients[0] == 0 && coefficients[1] == 0) || (coefficients[3] == 0 && coefficients[4] == 0))
            {
                return false;
            }

            var solver = new LinearSimultaneousEquationsAnalysisFunctions();
            var solution = solver.CalculateSolution(coefficients);

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

        protected override Func<List<int>, List<LinearSimultaneousEquationsSolution>, LinearSimultaneousEquations> ComputeContructorForQuestion()
        {
            return (coefficients, solutions) => new LinearSimultaneousEquations(coefficients, solutions);
        }

        protected override Func<LinearSimultaneousEquationsGeneratorParameters> ComputeContructorForParameters()
        {
            return () => new LinearSimultaneousEquationsGeneratorParameters();
        }
    }
}
