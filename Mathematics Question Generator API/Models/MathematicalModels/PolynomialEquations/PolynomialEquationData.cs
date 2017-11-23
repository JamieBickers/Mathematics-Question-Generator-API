using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquationData
    {
        public List<int> Coefficients;
        public List<RealRoot> RealRoots;
        public bool HasComplexRoot;
        public int Discriminant;

        public PolynomialEquationData(List<int> coefficients, List<RealRoot> realRoots, bool hasComplexRoot, int discriminant)
        {
            Coefficients = coefficients;
            RealRoots = realRoots;
            HasComplexRoot = hasComplexRoot;
            Discriminant = discriminant;
        }

        public bool MatchesCriteria(PolynomialEquationGeneratorParameters parameters)
        {
            int numberOfNonZeroCoefficients = Coefficients.Count(coefficient => coefficient != 0);

            if (Coefficients.Last() == 0)
            {
                return false;
            }
            // check not a single term polynomial, these are not interesting
            else if (numberOfNonZeroCoefficients == 1)
            {
                return false;
            }
            else if (numberOfNonZeroCoefficients < parameters.MinimumNumberOfTerms)
            {
                return false;
            }
            else if (parameters.LeadingTermLowerBound > Coefficients.Last() || parameters.LeadingTermUpperBound < Coefficients.Last())
            {
                return false;
            }
            else if (Coefficients.GetRange(0, Coefficients.Count() - 1).Min() < parameters.OtherTermsLowerBound
                || Coefficients.GetRange(0, Coefficients.Count() - 1).Max() > parameters.OtherTermsUpperBound)
            {
                return false;
            }
            else if (parameters.RequireAnIntegerRoot && !RealRoots.Exists(root => root.IsInteger))
            {
                return false;
            }
            else if (parameters.RequireARealRoot && RealRoots.Count == 0)
            {
                return false;
            }
            else if (parameters.RequireOnlyComplexRoots && RealRoots.Count() != 0)
            {
                return false;
            }
            else if (parameters.RequireAComplexRoot &&
                RealRoots.Select(root => root.Degree).Aggregate((first, next) => first + next) == parameters.Degree)
            {
                return false;
            }
            else if (parameters.RequireARealDoubleRoot && !RealRoots.Exists(root => root.Degree > 1))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
