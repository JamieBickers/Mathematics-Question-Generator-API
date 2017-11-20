using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;

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
            var equationGenerator = new QuadraticEquationGenerator(parameters, integerGenerator);

            var equation1 = equationGenerator.GenerateQuestionAndAnswer();
            var equation2 = equationGenerator.GenerateQuestionAndAnswer();
            var equation3 = equationGenerator.GenerateQuestionAndAnswer();

            Assert.AreEqual(9, equation1.Coefficients["a"]);
            Assert.AreEqual(50, equation1.Coefficients["b"]);
            Assert.AreEqual(51, equation1.Coefficients["c"]);

            Assert.AreEqual(3, equation2.Coefficients["a"]);
            Assert.AreEqual(44, equation2.Coefficients["b"]);
            Assert.AreEqual(-41, equation2.Coefficients["c"]);

            Assert.AreEqual(-3, equation3.Coefficients["a"]);
            Assert.AreEqual(-9, equation3.Coefficients["b"]);
            Assert.AreEqual(-58, equation3.Coefficients["c"]);
        }
    }
}