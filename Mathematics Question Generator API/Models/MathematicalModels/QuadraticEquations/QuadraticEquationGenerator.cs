using System;
using System.Collections.Generic;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGenerator : IQuestionGenerator<QuadraticEquation>
    {
        private QuadraticEquationGeneratorParameters parameters;
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private const int MaxNumberOfTries = 1000000;

        public QuadraticEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator)
        {
            var defaultParameters = new QuadraticEquationGeneratorParameters();
            parameters = defaultParameters;
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        public QuadraticEquationGenerator(QuadraticEquationGeneratorParameters parameters, IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.parameters = parameters;
        }

        public QuadraticEquation GenerateQuestionAndAnswer()
        {
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

            return roots;
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
                if (numberOfTries > MaxNumberOfTries)
                {
                    throw new Exception("Could not generate quadratic satisfying conditions.");
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
                int u = randomIntegerGenerator.GenerateRandomInteger(uUpperBound);
                int v = randomIntegerGenerator.GenerateRandomInteger(vUpperBound);
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
                { "a", randomIntegerGenerator.GenerateRandomInteger(parameters.ALowerBound, parameters.AUpperBound) },
                { "b", randomIntegerGenerator.GenerateRandomInteger(parameters.BLowerBound, parameters.BUpperBound) },
                { "c", randomIntegerGenerator.GenerateRandomInteger(parameters.CLowerBound, parameters.CUpperBound) }
            };

            return coefficients;
        }
    }
}
