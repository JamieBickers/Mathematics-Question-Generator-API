using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.LinearSimultaneousEquations
{
    public class LinearSimultaneousEquationsGeneratorParameters
    {
        public bool UniqueSolution;
        public bool NoSolutions;
        public bool InfiniteSolutions;
        public int CoefficientLowerBound;
        public int CoefficientUpperBound;

        public LinearSimultaneousEquationsGeneratorParameters(bool uniqueSolution = false, bool noSolutions = false,
            bool infiniteSolutions = false, int coefficientLowerBound = -100, int coefficientUpperBound = 100)
        {
            UniqueSolution = uniqueSolution;
            NoSolutions = noSolutions;
            InfiniteSolutions = infiniteSolutions;
            CoefficientLowerBound = coefficientLowerBound;
            CoefficientUpperBound = coefficientUpperBound;
            CheckValidParamaters();
        }

        private void CheckValidParamaters()
        {
            if (UniqueSolution && (NoSolutions || InfiniteSolutions))
            {
                throw new Exception("Cannot have unique solutions together with either no or infinite solutions.");
            }
            else if (NoSolutions && InfiniteSolutions)
            {
                throw new Exception("Cannot have no and infinite solutions.");
            }
            else if (CoefficientLowerBound > CoefficientUpperBound)
            {
                throw new Exception("Lower bound must be less than upper bound.");
            }
        }
    }
}
