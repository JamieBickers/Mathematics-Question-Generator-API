using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGenerator : IQuestionGenerator<LinearSimultaneousEquations>
    {
        private IRandomIntegerGenerator randomIntegerGenerator;
        private LinearSimultaneousEquationsGeneratorParameters parameters;

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
            do
            {
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

            if (!solution.InfiniteSolutions && parameters.InfiniteSolutions)
            {
                return false;
            }
            else if (!solution.NoSolution && parameters.NoSolutions)
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
