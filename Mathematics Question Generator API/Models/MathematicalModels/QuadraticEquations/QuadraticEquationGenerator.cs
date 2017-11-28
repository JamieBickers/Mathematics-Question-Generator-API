using System;
using System.Collections.Generic;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGenerator
        : QuestionGenerator<QuadraticEquation, QuadraticEquationGeneratorParameters, List<int>, List<double>>
    {
        public QuadraticEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }
        public QuadraticEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator, QuadraticEquationGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        public string GenerateQuestionAndAnswerAsString()
        {
            QuadraticEquation quadraticEquation = GenerateQuestionAndAnswer();
            return quadraticEquation.ParseToString();
        }

        protected override List<double> CalculateSolution(List<int> coefficients, out bool invalidCoefficients)
        {
            invalidCoefficients = false;
            int a = coefficients[0];
            int b = coefficients[1];
            int c = coefficients[2];

            var roots = QuadraticEquationAnalysisFunctions.ComputeRoots(a, b, c);

            return roots;
        }

        protected override List<int> GenerateValidCoefficients(out List<double> solution)
        {
            // for performance reasons generate double root equations separately
            return parameters.RequireDoubleRoot ? GenerateDoubleRootCoefficients(out solution) : base.GenerateValidCoefficients(out solution);
        }

        /* *
         * Instead of generating the coefficients and checking for a double root, we generate terms u and v
         * and use these to generate the quadratic (ux+v)^2, which always has a double root. We have the
         * restriction that u^2 <= aUpper and v^2 <= cUpper which helps narrow down the possible valid values.
         * This also means we only need to check for b being in range.
         * */
        private List<int> GenerateDoubleRootCoefficients(out List<double> solution)
        {
            var coefficients = new List<int>();
            bool invalidCoefficients;

            do
            {
                int uUpperBound = (int)Math.Round(Math.Sqrt(parameters.AUpperBound));
                int vUpperBound = (int)Math.Round(Math.Sqrt(parameters.CUpperBound));
                int u = randomIntegerGenerator.GenerateRandomInteger(uUpperBound);
                int v = randomIntegerGenerator.GenerateRandomInteger(vUpperBound);
                coefficients.Add(u * u);
                coefficients.Add(2 * u * v);
                coefficients.Add(v * v);
                solution = CalculateSolution(coefficients, out invalidCoefficients);
            } while (!CheckValidQuestion(coefficients, solution) || (coefficients[1] > parameters.BUpperBound)
            || (coefficients[1] < parameters.BLowerBound) || invalidCoefficients);

            return coefficients;
        }

        protected override bool CheckValidQuestion(List<int> coefficients, List<double> solution)
        {
            var a = coefficients[0];
            var b = coefficients[1];
            var c = coefficients[2];

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
            else if (parameters.RequireIntegerRoot && !(Math.Abs(solution[0] % 1) < 0.0001 && Math.Abs(solution[1] % 1) < 0.0001))
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
            var coefficients = new List<int>
            {
                randomIntegerGenerator.GenerateRandomInteger(parameters.ALowerBound, parameters.AUpperBound),
                randomIntegerGenerator.GenerateRandomInteger(parameters.BLowerBound, parameters.BUpperBound),
                randomIntegerGenerator.GenerateRandomInteger(parameters.CLowerBound, parameters.CUpperBound)
            };

            return coefficients;
        }

        protected override Func<List<int>, List<double>, QuadraticEquation> ComputeContructorForQuestion()
        {
            return (coefficients, solutions) => new QuadraticEquation(coefficients, solutions);
        }

        protected override Func<QuadraticEquationGeneratorParameters> ComputeContructorForParameters()
        {
            return () => new QuadraticEquationGeneratorParameters();
        }
    }
}
