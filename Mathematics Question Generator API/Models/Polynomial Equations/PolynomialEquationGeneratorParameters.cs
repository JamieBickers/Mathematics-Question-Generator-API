using System;

namespace Mathematics_Questions_Generator.Model.Polynomial_Equations
{
    class PolynomialEquationGeneratorParameters
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

        private void CheckValidParameters()
        {
            if ((LeadingTermLowerBound > LeadingTermUpperBound) || (OtherTermsLowerBound > OtherTermsUpperBound))
            {
                throw new Exception("Invalid bounds");
            }
            else if (Degree <= 0)
            {
                throw new Exception("Degree must be positive.");
            }
            else if (RequireOnlyComplexRoots && RequireARealRoot)
            {
                throw new Exception("Cannot have a real root as well as only complex roots.");
            }
            else if (RequireARealDoubleRoot && RequireOnlyComplexRoots)
            {
                throw new Exception("Cannot have a real double root as well as only complex roots.");
            }
        }
    }
}
