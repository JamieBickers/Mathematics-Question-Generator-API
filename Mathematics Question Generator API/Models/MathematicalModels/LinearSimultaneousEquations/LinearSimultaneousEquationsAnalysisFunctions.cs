using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.LinearSimultaneousEquations
{
    public class LinearSimultaneousEquationsAnalysisFunctions
    {
        public LinearSimultaneousEquationsSolution CalculateSolution(LinearEquation first, LinearEquation second)
        {
            var a = first.XTerm;
            var b = first.YTerm;
            var c = first.ConstantTerm;
            var d = second.XTerm;
            var e = second.YTerm;
            var f = second.ConstantTerm;

            var NaN = Double.NaN;
            var noSolution = new LinearSimultaneousEquationsSolution(NaN, NaN, true, false);

            if ((a == 0 && b == 0) || (d == 0 && e == 0))
            {
                throw new Exception("Invalid equation.");
            }
            else if (CheckIfParallel(first, second))
            {
                return GivenParallelCheckIfInfiniteSolutions(first, second) ? ComputeInfiniteSolution(first, second) : noSolution;
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

        private bool GivenParallelCheckIfInfiniteSolutions(LinearEquation first, LinearEquation second)
        {
            var a = first.XTerm;
            var b = first.YTerm;
            var c = first.ConstantTerm;
            var d = second.XTerm;
            var e = second.YTerm;
            var f = second.ConstantTerm;

            if (a != 0)
            {
                return Divide(c, a) == Divide(f, d);
            }
            else
            {
                return Divide(c, b) == Divide(f, e);
            }
        }

        private LinearSimultaneousEquationsSolution ComputeInfiniteSolution(LinearEquation first, LinearEquation second)
        {
            var a = first.XTerm;
            var b = first.YTerm;
            var c = first.ConstantTerm;
            var d = second.XTerm;
            var e = second.YTerm;
            var f = second.ConstantTerm;

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

        private bool CheckIfParallel(LinearEquation first, LinearEquation second)
        {
            var a = first.XTerm;
            var b = first.YTerm;
            var c = first.ConstantTerm;
            var d = second.XTerm;
            var e = second.YTerm;
            var f = second.ConstantTerm;

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
