using MathematicsQuestionGeneratorAPI.Exceptions;
using MathematicsQuestionGeneratorAPI.Models.PolynomialEquations;
using PolyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations
{
    public static class PolynomialEquationAnalysisFunctions
    {
        public static PolynomialEquationSolution ComputePolynomialEquationSolution(List<int> coefficients)
        {
            var polynomial = new Polynomial(coefficients.Select(val => (double)val).ToArray());
            var roots = polynomial.Roots();

            var rootsWithDegrees = ComputeDegrees(roots.Select(root => new Root(root.Re, root.Im, 1, false)));

            foreach (var root in rootsWithDegrees)
            {
                root.IsInteger = IsRootAnInteger(coefficients, root);
            }

            return new PolynomialEquationSolution(rootsWithDegrees.ToList(), ComputeDiscriminant(coefficients, rootsWithDegrees.ToList()));
        }

        private static IEnumerable<Root> ComputeDegrees(IEnumerable<Root> roots)
        {
            var withDegrees = new List<Root>();

            foreach (var root in roots)
            {
                var foundEqualRoot = false;

                foreach (var rootWithDegree in withDegrees)
                {
                    if (RootValueEquality(rootWithDegree, root))
                    {
                        rootWithDegree.Degree++;
                        foundEqualRoot = true;
                        break;
                    }
                }

                if (!foundEqualRoot)
                {
                    withDegrees.Add(root);
                }
            }

            return withDegrees;
        }

        private static bool IsRootAnInteger(List<int> coefficients, Root root)
        {
            // if not close to an integer or has imaginary part then return false immediatly
            if (Math.Abs(Math.Round(root.ImaginaryPart)) > 0.0001)
            {
                return false;
            }
            else if (Math.Abs(Math.Round(root.RealPart) % 1) > 0.0001)
            {
                return false;
            }

            var closestInteger = (int) Math.Round(root.RealPart);

            var evaluateAtClosestInteger = 0.0;
            for (var i = 0; i < coefficients.Count; i++)
            {
                evaluateAtClosestInteger += coefficients[i] * Math.Pow((double) closestInteger, (double) i);
            }

            return Math.Abs(evaluateAtClosestInteger % 1) < 0.0001;
        }

        private static int ComputeDiscriminant(List<int> coefficients, List<Root> roots)
        {
            // if any repeated roots return 0
            if (roots.Exists(root => root.Degree > 1))
            {
                return 0;
            }

            var discriminant = new System.Numerics.Complex(1, 0);
            for (var j = 0; j < roots.Count; j++)
            {
                for (var i = 0; i < j; i++)
                {
                    var ri = new System.Numerics.Complex(roots[i].RealPart, roots[i].ImaginaryPart);
                    var rj = new System.Numerics.Complex(roots[j].RealPart, roots[j].ImaginaryPart);
                    discriminant *= System.Numerics.Complex.Pow(ri - rj, 2);
                }
            }

            discriminant *= Math.Pow(coefficients[coefficients.Count - 1], 2 * (coefficients.Count - 1) - 2);

            if (!DoubleEquality(discriminant.Imaginary, 0, Math.Max(Math.Abs(discriminant.Real) * 0.0001, 0.0001)))
            {
                throw new MathematicalImpossibilityException();
            }

            return (int) Math.Round(discriminant.Real);
        }

        // TODO: Make comparing doubles much nicer and more modular
        private static bool DoubleEquality(double a, double b)
        {
            return DoubleEquality(a, b, 0.0001);
        }

        private static bool DoubleEquality(double a, double b, double precision)
        {
            return Math.Abs(a - b) < new List<double>()
            {
                precision * Math.Abs(a),
                precision * Math.Abs(b),
                precision
            }
            .Max();
        }

        private static bool RootValueEquality(Root a, Root b)
        {
            return DoubleEquality(a.RealPart, b.RealPart) && DoubleEquality(a.ImaginaryPart, b.ImaginaryPart);
        }
    }
}