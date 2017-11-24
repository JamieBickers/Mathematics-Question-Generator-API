using System;
using System.Collections.Generic;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGenerator : IQuestionGenerator<QuadraticEquation>
    {
        private const int MAX_NUMBER_OF_TRIES = 1000000;

        private QuadraticEquationGeneratorParameters parameters;
        private IRandomIntegerGenerator randomNumberGenerator;

        public QuadraticEquationGenerator(IRandomIntegerGenerator randomNumberGenerator)
        {
            var defaultParameters = new QuadraticEquationGeneratorParameters();
            parameters = defaultParameters;
            this.randomNumberGenerator = randomNumberGenerator;
        }

        public QuadraticEquationGenerator(QuadraticEquationGeneratorParameters parameters, IRandomIntegerGenerator randomNumberGenerator)
        {
            this.parameters = parameters;
            this.randomNumberGenerator = randomNumberGenerator;
        }

        public QuadraticEquation GenerateQuestionAndAnswer()
        {
            if (!parameters.CheckValidParameters())
            {
                throw new Exception("Invalid parameters.");
            }
            var coefficients = GenerateValidcoefficients();
            List<double> roots = CalculateRoots(coefficients);
            QuadraticEquation quadraticEquation = new QuadraticEquation(coefficients, roots);
            return quadraticEquation;
        }

        public string GenerateQuestionAndAnswerAsString()
        {
            QuadraticEquation quadraticEquation = GenerateQuestionAndAnswer();
            return quadraticEquation.ParseToString();
        }

        private List<double> CalculateRoots(Dictionary<string, int> coefficients)
        {
            int a = coefficients["a"];
            int b = coefficients["b"];
            int c = coefficients["c"];

            var roots = QuadraticEquationAnalysisFunctions.ComputeRoots(a, b, c);

            var roundedRoots = roots.Select(root => Math.Round(root, parameters.DecimalPlaces)).ToList();

            return roundedRoots;
        }

        private Dictionary<string, int> GenerateValidcoefficients()
        {
            Dictionary<string, int> coefficients;

            // for performance reasons generate double root equations separately
            if (parameters.RequireDoubleRoot)
            {
                return GenerateDoubleRootCoefficients();
            }

            var numberOfTries = 0;

            do
            {
                if (numberOfTries > MAX_NUMBER_OF_TRIES)
                {
                    throw new Exception("Could not generate polynomial satisfying conditions.");
                }
                coefficients = GenerateRandomcoefficients();
                numberOfTries++;
            } while (!CheckValidcoefficients(coefficients));

            return coefficients;
        }

        /* *
         * Instead of generating the coefficients and checking for a double root, we generate terms u and v
         * and use these to generate the quadratic (ux+v)^2, which always has a double root. We have the
         * restriction that u^2 <= aUpper and v^2 <= cUpper which helps narrow down the possible valid values.
         * This also means we only need to check for b being in range.
         * */
        private Dictionary<string, int> GenerateDoubleRootCoefficients()
        {
            var coefficients = new Dictionary<string, int>();

            do
            {
                int uUpperBound = (int)Math.Round(Math.Sqrt(parameters.AUpperBound));
                int vUpperBound = (int)Math.Round(Math.Sqrt(parameters.CUpperBound));
                int u = randomNumberGenerator.GenerateRandomInteger(uUpperBound);
                int v = randomNumberGenerator.GenerateRandomInteger(vUpperBound);
                coefficients["a"] = u * u;
                coefficients["b"] = 2 * u * v;
                coefficients["c"] = v * v;
            } while (!CheckValidcoefficients(coefficients) || (coefficients["b"] > parameters.BUpperBound) || (coefficients["b"] < parameters.BLowerBound));

            return coefficients;
        }

        private bool CheckValidcoefficients(Dictionary<string, int> coefficients)
        {
            var a = coefficients["a"];
            var b = coefficients["b"];
            var c = coefficients["c"];

            var discriminant = QuadraticEquationAnalysisFunctions.ComputeDiscriminant(a, b, c);

            if ((a == 0) || ((b == 0) && (c == 0)))
            {
                return false;
            }
            else if (parameters.RequireRealRoot && (discriminant < 0))
            {
                return false;
            }
            else if (parameters.RequireComplexRoot && (discriminant >= 0))
            {
                return false;
            }
            else if (parameters.RequireDoubleRoot && (discriminant != 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Dictionary<string, int> GenerateRandomcoefficients()
        {
            var coefficients = new Dictionary<string, int>
            {
                { "a", randomNumberGenerator.GenerateRandomInteger(parameters.ALowerBound, parameters.AUpperBound) },
                { "b", randomNumberGenerator.GenerateRandomInteger(parameters.BLowerBound, parameters.BUpperBound) },
                { "c", randomNumberGenerator.GenerateRandomInteger(parameters.CLowerBound, parameters.CUpperBound) }
            };

            return coefficients;
        }
    }
}
