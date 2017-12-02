using MathematicsQuestionGeneratorAPI.Models.PolynomialEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations
{
    public class PolynomialEquationSolution
    {
        public List<Root> Roots;
        public int Discriminant;

        public PolynomialEquationSolution(List<Root> roots, int discriminant)
        {
            Roots = roots;
            Discriminant = discriminant;
        }
    }
}
