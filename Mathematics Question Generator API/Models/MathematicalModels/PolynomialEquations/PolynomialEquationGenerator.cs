using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquationGenerator : QuestionGenerator<PolynomialEquation, PolynomialEquationGeneratorParameters, List<int>, PolynomialEquationSolution>
    {
        public PolynomialEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }
        public PolynomialEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator, PolynomialEquationGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        // Stub, implementing this will be a mini project in itself.
        protected override PolynomialEquationSolution CalculateSolution(List<int> coefficients, out bool invalidCoefficients)
        {
            invalidCoefficients = false;
            return new PolynomialEquationSolution(new List<RealRoot>(), false, 0);
        }

        // For testing reasons don't implement until solutions can be calculated.
        protected override bool CheckValidQuestion(List<int> coefficients, PolynomialEquationSolution solution)
        {
            return true;
        }

        protected override Func<PolynomialEquationGeneratorParameters> ComputeContructorForParameters()
        {
            return () => new PolynomialEquationGeneratorParameters();
        }

        protected override Func<List<int>, PolynomialEquationSolution, PolynomialEquation> ComputeContructorForQuestion()
        {
            return (coefficients, solution) => new PolynomialEquation(coefficients, solution);
        }

        protected override List<int> GenerateRandomCoefficients()
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
