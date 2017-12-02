using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquationGenerator : QuestionGenerator<PolynomialEquation, PolynomialEquationGeneratorParameters, List<int>, PolynomialEquationSolution>
    {
        public PolynomialEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator) : base(randomIntegerGenerator) { }
        public PolynomialEquationGenerator(IRandomIntegerGenerator randomIntegerGenerator, PolynomialEquationGeneratorParameters parameters)
            : base(randomIntegerGenerator, parameters) { }

        protected override PolynomialEquationSolution CalculateSolution(List<int> coefficients, out bool invalidCoefficients)
        {
            if (coefficients[coefficients.Count - 1] == 0 || coefficients.Count(term => term != 0) < parameters.MinimumNumberOfTerms)
            {
                invalidCoefficients = true;
                return new PolynomialEquationSolution(new List<Root>(), 0);
            }

            invalidCoefficients = true;
            return PolynomialEquationAnalysisFunctions.ComputePolynomialEquationSolution(coefficients);
        }

        protected override bool CheckValidQuestion(List<int> coefficients, PolynomialEquationSolution solution)
        {
            if (parameters.RequireAnIntegerRoot && !solution.Roots.Exists(root => root.IsInteger))
            {
                return false;
            }
            else if (parameters.RequireARealRoot && !solution.Roots.Exists(root => root.ImaginaryPart == 0))
            {
                return false;
            }
            else if (parameters.RequireAComplexRoot && !solution.Roots.Exists(root => root.ImaginaryPart != 0))
            {
                return false;
            }
            else if (parameters.RequireOnlyComplexRoots && solution.Roots.Exists(root => root.ImaginaryPart == 0))
            {
                return false;
            }
            else if (parameters.RequireADoubleRoot && !solution.Roots.Exists(root => root.Degree > 1))
            {
                return false;
            }
            else
            {
                return true;
            }
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
            var randomNumberGenerator = new Random();
            var coefficients = new List<int>();

            // Add the non-leading coefficients
            for (var i = 0; i < parameters.Degree; i++)
            {
                coefficients.Add(randomNumberGenerator.Next(parameters.OtherTermsLowerBound, parameters.OtherTermsUpperBound));
            }

            // Add the leading coefficient
            coefficients.Add(randomNumberGenerator.Next(parameters.LeadingTermLowerBound, parameters.LeadingTermUpperBound));

            return coefficients;
        }
    }
}
