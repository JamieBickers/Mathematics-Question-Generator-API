using MathematicsQuestionGeneratorAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public static class LinearSimultaneousEquationsAnalysisFunctions
    {
        public static LinearSimultaneousEquationsSolution CalculateSolution(List<LinearEquation> coefficients, out bool invalidCoefficients)
        {
            var a = coefficients[0].XTerm;
            var b = coefficients[0].YTerm;
            var c = coefficients[0].ConstantTerm;
            var d = coefficients[1].XTerm;
            var e = coefficients[1].YTerm;
            var f = coefficients[1].ConstantTerm;

            var NaN = Double.NaN;
            var noSolution = new LinearSimultaneousEquationsSolution(NaN, NaN, true, false);
            invalidCoefficients = false;

            if ((a == 0 && b == 0) || (d == 0 && e == 0))
            {
                invalidCoefficients = true;
                return new LinearSimultaneousEquationsSolution(Double.NaN, Double.NaN, false, false);
            }
            else if (CheckIfParallel(coefficients))
            {
                return GivenParallelCheckIfInfiniteSolutions(coefficients) ? ComputeInfiniteSolution(coefficients) : noSolution;
            }
            else if (a != 0)
            {
                return new LinearSimultaneousEquationsSolution(Divide(b * f - c * e, e * a - b * d), Divide(c * d - a * f, e * a - b * d), false, false);
            }
            else if (b != 0 && d != 0)
            {
                return new LinearSimultaneousEquationsSolution(Divide(c * e - f * b, b * d), Divide(-c, b), false, false);
            }
            else
            {
                throw new MathematicalImpossibilityException();
            }
        }

        private static bool GivenParallelCheckIfInfiniteSolutions(List<LinearEquation> coefficients)
        {
            var a = coefficients[0].XTerm;
            var b = coefficients[0].YTerm;
            var c = coefficients[0].ConstantTerm;
            var d = coefficients[1].XTerm;
            var e = coefficients[1].YTerm;
            var f = coefficients[1].ConstantTerm;

            if (a != 0)
            {
                return Divide(c, a) == Divide(f, d);
            }
            else
            {
                return Divide(c, b) == Divide(f, e);
            }
        }

        private static LinearSimultaneousEquationsSolution ComputeInfiniteSolution(List<LinearEquation> coefficients)
        {
            var a = coefficients[0].XTerm;
            var b = coefficients[0].YTerm;
            var c = coefficients[0].ConstantTerm;
            var d = coefficients[1].XTerm;
            var e = coefficients[1].YTerm;
            var f = coefficients[1].ConstantTerm;

            if (a != 0)
            {
                return new LinearSimultaneousEquationsSolution(Divide(- c - b, a), 1, false, true);
            }
            else if (d != 0)
            {
                return new LinearSimultaneousEquationsSolution(Divide(-f - e, d), 1, false, true);
            }
            else
            {
                return new LinearSimultaneousEquationsSolution(1, Divide(-c, b), false, true);
            }
        }

        private static bool CheckIfParallel(List<LinearEquation> coefficients)
        {
            var a = coefficients[0].XTerm;
            var b = coefficients[0].YTerm;
            var c = coefficients[0].ConstantTerm;
            var d = coefficients[1].XTerm;
            var e = coefficients[1].YTerm;
            var f = coefficients[1].ConstantTerm;

            if (b != 0 && e != 0)
            {
                return Divide(a, b) == Divide(d, e);
            }
            else if (b == 0)
            {
                return e == 0;
            }
            else if (e == 0)
            {
                return b == 0;
            }

            throw new MathematicalImpossibilityException();
        }

        private static double Divide(int a, int b)
        {
            return Convert.ToDouble(a) / Convert.ToDouble(b);
        }
    }
}
