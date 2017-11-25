using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsAnalysisFunctions
    {
        public LinearSimultaneousEquationsSolution CalculateSolution(List<int> coefficients)
        {
            var a = coefficients[0];
            var b = coefficients[1];
            var c = coefficients[2];
            var d = coefficients[3];
            var e = coefficients[4];
            var f = coefficients[5];

            var NaN = Double.NaN;
            var noSolution = new LinearSimultaneousEquationsSolution(NaN, NaN, true, false);

            if ((a == 0 && b == 0) || (d == 0 && e == 0))
            {
                throw new Exception("Invalid equation.");
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
                throw new Exception("Mathematically impossible.");
            }
        }

        private bool GivenParallelCheckIfInfiniteSolutions(List<int> coefficients)
        {
            var a = coefficients[0];
            var b = coefficients[1];
            var c = coefficients[2];
            var d = coefficients[3];
            var e = coefficients[4];
            var f = coefficients[5];

            if (a != 0)
            {
                return Divide(c, a) == Divide(f, d);
            }
            else
            {
                return Divide(c, b) == Divide(f, e);
            }
        }

        private LinearSimultaneousEquationsSolution ComputeInfiniteSolution(List<int> coefficients)
        {
            var a = coefficients[0];
            var b = coefficients[1];
            var c = coefficients[2];
            var d = coefficients[3];
            var e = coefficients[4];
            var f = coefficients[5];

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

        private bool CheckIfParallel(List<int> coefficients)
        {
            var a = coefficients[0];
            var b = coefficients[1];
            var c = coefficients[2];
            var d = coefficients[3];
            var e = coefficients[4];
            var f = coefficients[5];

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

            throw new Exception("Impossible situation.");
        }

        private double Divide(int a, int b)
        {
            return Convert.ToDouble(a) / Convert.ToDouble(b);
        }
    }
}
