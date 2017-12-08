using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.PolynomialEquations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorTests.Models.PolynomialEquations
{
    [TestClass]
    public class PolynomialEquationAnalysisFunctionsTests
    {
        [TestMethod]
        public void CheckFindsTripleRoot()
        {
            var polynomial = new List<int>() { 1, 3, 3, 1 };

            var solution = PolynomialEquationAnalysisFunctions.ComputePolynomialEquationSolution(polynomial);

            Assert.AreEqual(3, solution.Roots.First().Degree);
        }

        [TestMethod]
        public void CheckFindsIntegerRoot()
        {
            var polynomial = new List<int>() { 1, 2, 2, 1 };

            var solution = PolynomialEquationAnalysisFunctions.ComputePolynomialEquationSolution(polynomial);

            Assert.IsTrue(solution.Roots.Exists(root => root.IsInteger));
        }

        [TestMethod]
        public void CheckComputesDiscriminant()
        {
            var polynomial = new List<int>() { 1, -9, 1, 2, -6, 1 };

            var solution = PolynomialEquationAnalysisFunctions.ComputePolynomialEquationSolution(polynomial);

            Assert.AreEqual(-158315240, solution.Discriminant);
        }
    }
}