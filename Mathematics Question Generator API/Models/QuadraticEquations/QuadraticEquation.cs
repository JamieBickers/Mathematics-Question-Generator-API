using System.Collections.Generic;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquation
    {
        public Dictionary<string, int> Coefficients;

        public List<double> Roots;

        public QuadraticEquation(Dictionary<string, int> coefficients, List<double> roots)
        {
            Coefficients = coefficients;
            Roots = roots;
        }
    }
}
