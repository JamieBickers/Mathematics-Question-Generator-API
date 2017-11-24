using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGenerator : IQuestionGenerator<LinearSimultaneousEquations>
    {
        private LinearSimultaneousEquationsGeneratorParameters parameters;
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private const int MaxNumberOfTries = 1000000;

        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            parameters = new LinearSimultaneousEquationsGeneratorParameters();
        }

        public LinearSimultaneousEquationsGenerator(IRandomIntegerGenerator randomIntegerGenerator, LinearSimultaneousEquationsGeneratorParameters parameters)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.parameters = parameters;
        }

        public LinearSimultaneousEquations GenerateQuestionAndAnswer()
        {
            var equations = GenerateValidEquations();
            var solver = new LinearSimultaneousEquationsAnalysisFunctions();
            var solution = solver.CalculateSolution(equations[0], equations[1]);
            return new LinearSimultaneousEquations(equations[0], equations[1], solution);
        }

        private List<LinearEquation> GenerateValidEquations()
        {
            LinearEquation first;
            LinearEquation second;

            var numberOfTries = 0;
            do
            {
                if (numberOfTries > MaxNumberOfTries)
                {
                    throw new Exception("Could not generate equations satisfying conditions.");
                }
                first = GenerateRandomEquation();
                second = GenerateRandomEquation();
            } while (!CheckValidEquations(first, second));

            return new List<LinearEquation>() { first, second };
        }

        private bool CheckValidEquations(LinearEquation first, LinearEquation second)
        {
            if ((first.XTerm == 0 && first.YTerm == 0) || (second.XTerm == 0 && second.YTerm == 0))
            {
                return false;
            }

            var solver = new LinearSimultaneousEquationsAnalysisFunctions();
            var solution = solver.CalculateSolution(first, second);

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

        private LinearEquation GenerateRandomEquation()
        {
            var xTerm = randomIntegerGenerator.GenerateRandomInteger(parameters.CoefficientLowerBound, parameters.CoefficientUpperBound);
            var yTerm = randomIntegerGenerator.GenerateRandomInteger(parameters.CoefficientLowerBound, parameters.CoefficientUpperBound);
            var constantTerm = randomIntegerGenerator.GenerateRandomInteger(parameters.CoefficientLowerBound, parameters.CoefficientUpperBound);

            return new LinearEquation(xTerm, yTerm, constantTerm);
        }
    }
}
