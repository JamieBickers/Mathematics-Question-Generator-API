using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public static class QuadraticEquationAnalysisFunctions
    {
        public static int ComputeDiscriminant(int a, int b, int c)
        {
            return b * b - 4 * a * c;
        }

        public static List<double> ComputeRoots(int a, int b, int c)
        {
            return new List<double>() { (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a), (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a) };
        }
    }
}
