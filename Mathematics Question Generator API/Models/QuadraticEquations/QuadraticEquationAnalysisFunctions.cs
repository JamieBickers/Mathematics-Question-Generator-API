using System;
using System.Collections.Generic;

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
            var discriminant = ComputeDiscriminant(a, b, c);
            return new List<double>() { (-b + Math.Sqrt(discriminant)) / (2 * a), (-b - Math.Sqrt(discriminant)) / (2 * a) };
        }
    }
}
