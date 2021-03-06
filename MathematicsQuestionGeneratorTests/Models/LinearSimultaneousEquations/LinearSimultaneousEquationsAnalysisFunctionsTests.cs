﻿using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathematicsQuestionGeneratorTests.Models.SimultaneousEquations
{
    [TestClass]
    public class LinearSimultaneousEquationsAnalysisFunctionsTests
    {
        [TestMethod]
        public void TestFindsUniqueSolution()
        {
            var firstEquation = new LinearEquation(3, 2, -36);
            var secondEquation = new LinearEquation(5, 4, -64);
            var coefficients = new List<LinearEquation>() { firstEquation, secondEquation };

            bool invalidCoefficients;
            var actualSolution = LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients, out invalidCoefficients);
            var expectedSolution = new LinearSimultaneousEquationsSolution(8, 6, false, false);

            Assert.IsTrue(EqualSolutions(expectedSolution, actualSolution));
            Assert.IsFalse(invalidCoefficients);
        }

        [TestMethod]
        public void TestFindsNoSolution()
        {
            var firstEquation = new LinearEquation(18, -3, 46);
            var secondEquation = new LinearEquation(6, -1, 12);
            var coefficients = new List<LinearEquation>() { firstEquation, secondEquation };

            bool invalidCoefficients;
            var actualSolution = LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients, out invalidCoefficients);
            var expectedSolution = new LinearSimultaneousEquationsSolution(Double.NaN, Double.NaN, true, false);

            Assert.IsTrue(EqualSolutions(expectedSolution, actualSolution));
        }

        [TestMethod]
        public void TestFindsInfiniteSolutions()
        {
            var firstEquation = new LinearEquation(18, -3, 45);
            var secondEquation = new LinearEquation(6, -1, 15);
            var coefficients = new List<LinearEquation>() { firstEquation, secondEquation };

            bool invalidCoefficients;
            var actualSolution = LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(coefficients, out invalidCoefficients);
            var expectedSolution = new LinearSimultaneousEquationsSolution(Convert.ToDouble(-14) / Convert.ToDouble(6), 1, false, true);

            Assert.IsTrue(EqualSolutions(expectedSolution, actualSolution));
        }

        [TestMethod]
        public void TestLotsOfSimultaneousEquations()
        {
            var integerGenerator = new FixedRandomIntegerGenerator(346);
            var equationGenerator = new LinearSimultaneousEquationsGenerator(integerGenerator);

            for (int i = 0; i < 1000000; i++)
            {
                var equation = equationGenerator.GenerateQuestionAndAnswer();
                var isCorrect = VerifySolutionIfSolutionExists(equation.Coefficients, equation.Solution);
                Assert.IsTrue(isCorrect, $"{i}");
            }
        }

        private bool EqualSolutions(LinearSimultaneousEquationsSolution first, LinearSimultaneousEquationsSolution second)
        {
            var b1 = first.FirstSolution == second.FirstSolution || (Double.IsNaN(first.FirstSolution) && Double.IsNaN(second.FirstSolution));
            var b2 = first.SecondSolution == second.SecondSolution || (Double.IsNaN(first.SecondSolution) && Double.IsNaN(second.SecondSolution));
            var b3 = first.NoSolution == second.NoSolution;
            var b4 = first.InfiniteSolutions == second.InfiniteSolutions;

            return b1 && b2 && b3 && b4;
        }

        // returns true if solution doesn't exist
        private bool VerifySolutionIfSolutionExists(List<LinearEquation> coefficients, LinearSimultaneousEquationsSolution solution)
        {
            var a = coefficients[0].XTerm;
            var b = coefficients[0].YTerm;
            var c = coefficients[0].ConstantTerm;
            var d = coefficients[1].XTerm;
            var e = coefficients[1].YTerm;
            var f = coefficients[1].ConstantTerm;
            var x = solution.FirstSolution;
            var y = solution.SecondSolution;

            if (solution.NoSolution)
            {
                return true;
            }

            var firstValue = a * x + b * y + c;
            var secondValue = d * x + e * y + f;

            var firstSatisfied = Math.Abs(firstValue) < 0.001;
            var secondSatisfied = Math.Abs(secondValue) < 0.001;

            return firstSatisfied && secondSatisfied;
        }
    }
}
