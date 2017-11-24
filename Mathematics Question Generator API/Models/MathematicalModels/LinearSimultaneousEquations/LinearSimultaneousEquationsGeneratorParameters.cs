using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGeneratorParameters : QuestionParameters
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
            if (!CheckValidParameters())
            {
                throw new Exception("Invalid parameters.");
            }
        }

        protected override bool CheckValidParameters()
        {
            if (UniqueSolution && (NoSolutions || InfiniteSolutions))
            {
                return false;
            }
            else if (NoSolutions && InfiniteSolutions)
            {
                return false;
            }
            else if (CoefficientLowerBound > CoefficientUpperBound)
            {
                return false;
            }
            return true;
        }
    }
}
