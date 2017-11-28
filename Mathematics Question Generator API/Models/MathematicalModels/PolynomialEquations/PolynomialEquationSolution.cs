using MathematicsQuestionGeneratorAPI.Models.PolynomialEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations
{
    public class PolynomialEquationSolution
    {
        public List<RealRoot> RealRoots;
        public bool HasComplexRoot;
        public int Discriminant;

        public PolynomialEquationSolution(List<RealRoot> realRoots, bool hasComplexRoot, int discriminant)
        {
            RealRoots = realRoots;
            HasComplexRoot = hasComplexRoot;
            Discriminant = discriminant;
        }
    }
}
