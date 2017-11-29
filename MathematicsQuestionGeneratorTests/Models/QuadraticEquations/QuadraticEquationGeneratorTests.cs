using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Exceptions;

namespace MathematicsQuestionGeneratorTests
{
    [TestClass]
    public class QuadraticEquationGeneratorTests
    {
        [TestMethod]
        public void ExpectSameCoefficentsEveryTime()
        {
            var integerGenerator = new FixedRandomIntegerGenerator(10);
            var parameters = new QuadraticEquationGeneratorParameters(aLowerBound: -10, aUpperBound: 10, bLowerBound: -100, bUpperBound: 100,
                cLowerBound: -100, cUpperBound: 100);
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator, parameters);

            var equation1 = equationGenerator.GenerateQuestionAndAnswer();
            var equation2 = equationGenerator.GenerateQuestionAndAnswer();
            var equation3 = equationGenerator.GenerateQuestionAndAnswer();

            Assert.AreEqual(9, equation1.Coefficients[0]);
            Assert.AreEqual(50, equation1.Coefficients[1]);
            Assert.AreEqual(51, equation1.Coefficients[2]);

            Assert.AreEqual(3, equation2.Coefficients[0]);
            Assert.AreEqual(44, equation2.Coefficients[1]);
            Assert.AreEqual(-41, equation2.Coefficients[2]);

            Assert.AreEqual(-3, equation3.Coefficients[0]);
            Assert.AreEqual(-9, equation3.Coefficients[1]);
            Assert.AreEqual(-58, equation3.Coefficients[2]);
        }

        [TestMethod]
        public void CheckDoesNotThrowErrorWhenBoundsAreValid()
        {
            var parameters = new QuadraticEquationGeneratorParameters(aLowerBound: 4, aUpperBound: 5, bLowerBound: 765,
                bUpperBound: 800, cLowerBound: -32, cUpperBound: -27);
            var integerGenerator = new FixedRandomIntegerGenerator(5);
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator, parameters);

            var equation = equationGenerator.GenerateQuestionAndAnswer();
        }

        [TestMethod]
        public void ExpectRealSolutionsWhenAskedFor()
        {
            var integerGenerator = new FixedRandomIntegerGenerator(71);
            var parameters = new QuadraticEquationGeneratorParameters(requireRealRoot: true);
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator, parameters);

            var equation = equationGenerator.GenerateQuestionAndAnswer();

            Assert.AreEqual(equation.Roots.Count(root => Double.IsNaN(root)), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedToGenerateQuestionSatisfyingParametersException))]
        public void ExpectExceptionWhenUsingImpossibleCondition()
        {
            var integerGenerator = new FixedRandomIntegerGenerator(71);

            // These parameters are mathematically impossible to fulfill as an equation
            // with such coefficients will have a real root. This is because f(0)>0, f(x) -> infinity
            // as |x| -> infinity
            var parameters = new QuadraticEquationGeneratorParameters(aLowerBound: 1, cUpperBound: -1, requireComplexRoot: true);
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator, parameters);

            equationGenerator.GenerateQuestionAndAnswer();
        }
    }
}