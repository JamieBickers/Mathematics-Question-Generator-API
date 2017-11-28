using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquation : IQuestion
    {
        public List<int> Coefficients;
        public PolynomialEquationSolution Solution;

        public PolynomialEquation(List<int> coefficients, PolynomialEquationSolution solution)
        {
            Coefficients = coefficients;
            Solution = solution;
        }

        /*public bool MatchesCriteria(PolynomialEquationGeneratorParameters parameters)
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
        }*/

        public PdfPCell ParseToPdfPCell(int questionNumber, bool showAnswers)
        {
            throw new System.NotImplementedException();
        }
    }
}
