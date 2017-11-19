using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGenerator : IQuestionGenerator<QuadraticEquation>
    {
        private const int MAX_NUMBER_OF_TRIES = 10000;

        private QuadraticEquationGeneratorParameters parameters;

        public QuadraticEquationGenerator()
        {
            var defaultParameters = new QuadraticEquationGeneratorParameters();
            parameters = defaultParameters;
        }

        public QuadraticEquationGenerator(QuadraticEquationGeneratorParameters parameters)
        {
            this.parameters = parameters;
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
            return Parser(quadraticEquation.Coefficients, quadraticEquation.Roots);
        }

        private List<double> CalculateRoots(Dictionary<string, int> coefficients)
        {
            int a = coefficients["a"];
            int b = coefficients["b"];
            int c = coefficients["c"];

            var roots = new List<double>() { (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a), (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a) };

            var roundedRoots = roots.Select(root => Math.Round(root, parameters.DecimalPlaces)).ToList();

            return roundedRoots;
        }

        private Dictionary<string, int> GenerateValidcoefficients()
        {
            Dictionary<string, int> coefficients;

            // for performance reasons generate double root equations separately
            if (parameters.RequireDoubleRoot)
            {
                return GenerateDoubleRootcoefficients();
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
        private Dictionary<string, int> GenerateDoubleRootcoefficients()
        {
            Random randomNumberGenerator = new Random();
            var coefficients = new Dictionary<string, int>();

            do
            {
                int uUpperBound = (int)Math.Round(Math.Sqrt(parameters.AUpperBound));
                int vUpperBound = (int)Math.Round(Math.Sqrt(parameters.CUpperBound));
                int u = randomNumberGenerator.Next(uUpperBound);
                int v = randomNumberGenerator.Next(vUpperBound);
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

            var discriminant = b * b - 4 * a * c;

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
            Random randomNumberGenerator = new Random();
            var coefficients = new Dictionary<string, int>
            {
                { "a", randomNumberGenerator.Next(parameters.ALowerBound, parameters.AUpperBound) },
                { "b", randomNumberGenerator.Next(parameters.BLowerBound, parameters.BUpperBound) },
                { "c", randomNumberGenerator.Next(parameters.CLowerBound, parameters.CUpperBound) }
            };

            return coefficients;
        }

        private static string Parser(Dictionary<string, int> coefficients, List<double> roots)
        {
            string aTerm = (coefficients["a"] == 1) ? "" : $"{coefficients["a"]}";
            string bTerm = (coefficients["b"] < 0) ? $"{coefficients["b"]}" : $"+{coefficients["b"]}";
            string cTerm = (coefficients["c"] < 0) ? $"{coefficients["c"]}" : $"+{coefficients["c"]}";

            return $"Question: {aTerm}x^2{bTerm}x{cTerm}=0\nRoots: {roots[0].ToString()}, {roots[1].ToString()}";
        }
    }
}
