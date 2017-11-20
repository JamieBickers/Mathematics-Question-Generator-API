using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorTests.Models.QuadraticEquations
{
    [TestClass]
    public class QuadraticEquationAnalysisFunctionsTests
    {
        [TestMethod]
        public void CheckRealRootsAreCalculatedCorrectly()
        {
            int a = 958, b = -324, c = -902;
            var exceptedRoots = new List<double>() { -0.816, 1.154 };

            var roots = QuadraticEquationAnalysisFunctions.ComputeRoots(a, b, c).Select(root => Math.Round(root, 3)).ToList();

            CollectionAssert.AreEquivalent(exceptedRoots, roots);
        }

        [TestMethod]
        public void CheckImaginaryRootsReturnNaN()
        {
            int a = 342563, b = -834, c = 22034;

            var roots = QuadraticEquationAnalysisFunctions.ComputeRoots(a, b, c);

            Assert.AreEqual(roots.Count(root => Double.IsNaN(root)), 2);
        }

        [TestMethod]
        public void CheckDiscriminantIsCorrect()
        {
            int a = 92347, b = -3252, c = 1432;

            var discriminant = QuadraticEquationAnalysisFunctions.ComputeDiscriminant(a, b, c);

            Assert.AreEqual(-518388112, discriminant);
        }
    }
}