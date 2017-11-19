using Mathematics_Questions_Generator.Model.Polynomial_Equations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathematics_Questions_Generator.Model
{
    class PolynomialEquationGenerator : IQuestionGenerator<PolynomialEquationData>
    {
        private PolynomialEquationGeneratorParameters parameters;

        public PolynomialEquationGenerator()
        {
            parameters = new PolynomialEquationGeneratorParameters();
        }

        public PolynomialEquationGenerator(PolynomialEquationGeneratorParameters parameters)
        {
            this.parameters = parameters;
        }

        /* *
         * The list of coefficients will start with the constant term, end with the leading term.
         * */
        public PolynomialEquationData GenerateQuestionAndAnswer()
        {
            PolynomialEquationData polynomialEquation;

            do
            {
                List<int> coefficients = GenerateRandomCoefficients();
                polynomialEquation = ComputePolynomialData(coefficients);
            } while (!polynomialEquation.MatchesCriteria(parameters));

            return polynomialEquation;
        }

        private PolynomialEquationData ComputePolynomialData(List<int> coefficients)
        {
            return new PolynomialEquationData(coefficients, new List<RealRoot>(), false, 0);
        }

        private List<int> GenerateRandomCoefficients()
        {
            Random randomNumberGenerator = new Random();
            List<int> coefficients = new List<int>();

            // Add the non-leading coefficients
            for (int i = 0; i < parameters.Degree; i++)
            {
                coefficients.Add(randomNumberGenerator.Next(parameters.OtherTermsLowerBound, parameters.OtherTermsUpperBound));
            }

            // Add the leading coefficient
            coefficients.Add(randomNumberGenerator.Next(parameters.LeadingTermLowerBound, parameters.LeadingTermUpperBound));

            return coefficients;
        }
    }
}
