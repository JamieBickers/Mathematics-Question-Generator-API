using MathematicsQuestionGeneratorAPI.Exceptions;
using System;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquationGeneratorParameters
    {
        public int Degree;
        public int MinimumNumberOfTerms;

        public int LeadingTermLowerBound;
        public int LeadingTermUpperBound;
        public int OtherTermsLowerBound;
        public int OtherTermsUpperBound;

        public bool RequireAnIntegerRoot;
        public bool RequireARealRoot;
        public bool RequireAComplexRoot;
        public bool RequireOnlyComplexRoots;
        public bool RequireARealDoubleRoot;

        public PolynomialEquationGeneratorParameters(int degree = 2, int leadingTermLowerBound = -10, int leadingTermUpperBound = 10, int otherTermsLowerBound = -100,
            int otherTermsUpperBound = 100, int minimumNumberOfTerms = -1,  bool requireAnIntegerRoot = false, bool requireARealRoot = false,
            bool requireAComplexRoot = false, bool requireOnlyComplexRoots = false, bool requireADoubleRoot = false)
        {
            Degree = degree;
            LeadingTermLowerBound = leadingTermLowerBound;
            LeadingTermUpperBound = leadingTermUpperBound;
            OtherTermsLowerBound = otherTermsLowerBound;
            OtherTermsUpperBound = otherTermsUpperBound;
            MinimumNumberOfTerms = minimumNumberOfTerms == -1 ? degree / 2 : minimumNumberOfTerms;
            RequireAnIntegerRoot = requireAnIntegerRoot;
            RequireARealRoot = requireARealRoot;
            RequireAComplexRoot = requireAComplexRoot;
            RequireOnlyComplexRoots = requireOnlyComplexRoots;
            RequireARealDoubleRoot = requireADoubleRoot;
        }

        // leave this here until I properly implement this question type
        private void CheckValidParameters()
        {
            if ((LeadingTermLowerBound > LeadingTermUpperBound) || (OtherTermsLowerBound > OtherTermsUpperBound))
            {
            }
            else if (Degree <= 0)
            {
            }
            else if (RequireOnlyComplexRoots && RequireARealRoot)
            {
            }
            else if (RequireARealDoubleRoot && RequireOnlyComplexRoots)
            {
            }
        }
    }
}
