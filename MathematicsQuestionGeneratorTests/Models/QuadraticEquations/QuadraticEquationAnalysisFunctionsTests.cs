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
            var exceptedSolutions = new List<double>() { -0.816, 1.154 };

            var solutions = QuadraticEquationAnalysisFunctions.ComputeRoots(a, b, c).Select(root => Math.Round(root, 3)).ToList();

            CollectionAssert.AreEquivalent(exceptedSolutions, solutions);
        }
    }
}