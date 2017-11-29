using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public static class LinearSimultaneousEquationsParser
    {
        public static string ParseToString(LinearSimultaneousEquations equations)
        {
            var firstParsed = ParseLinearEquationToString(equations.Coefficients[0]);
            var secondParsed = ParseLinearEquationToString(equations.Coefficients[0]);

            return $"{firstParsed}\n{secondParsed}=0\nx={equations.Solution.FirstSolution}, y={equations.Solution.SecondSolution}";
        }

        public static PdfPCell ParseToPdfParagraph(LinearSimultaneousEquations equations, bool showAnswers)
        {
            var firstParsed = ParseLinearEquationToString(equations.Coefficients[0]);
            var secondParsed = ParseLinearEquationToString(equations.Coefficients[0]);

            var firstEquation = new Chunk(firstParsed);
            var secondEquation = new Chunk(secondParsed);

            var question = new Paragraph
            {
                firstEquation,
                secondEquation
            };

            question.SpacingAfter = 100;

            var answerArea = CreateAnswerArea(equations, showAnswers);

            var questionAndAnswer = new PdfPCell();
            questionAndAnswer.AddElement(question);
            questionAndAnswer.AddElement(answerArea);
            questionAndAnswer.BorderColor = BaseColor.White;

            return questionAndAnswer;
        }

        private static Paragraph CreateAnswerArea(LinearSimultaneousEquations equations, bool showAnswers)
        {
            var xAnswer = showAnswers ? Math.Round(equations.Solution.FirstSolution, 2).ToString() : "........";
            var yAnswer = showAnswers ? Math.Round(equations.Solution.FirstSolution, 2).ToString() : "........";
            var answerArea = new Paragraph($"Answer: x={xAnswer}, y={yAnswer}");
            answerArea.Alignment = Element.ALIGN_RIGHT;
            return answerArea;
        }

        public static string ParseLinearEquationToString(LinearEquation equation)
        {
            var pairs = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("x", equation.XTerm),
                new KeyValuePair<string, int>("y", equation.YTerm),
                new KeyValuePair<string, int>("", equation.ConstantTerm)
            };


            return ParseCoefficientVariablePairList(pairs, pairs.Count);
        }

        private static string ParseCoefficientVariablePairList(List<KeyValuePair<string, int>> pairs, int startingLength)
        {
            var beforeIfPositive = startingLength == pairs.Count ? "" : "+";

            if (pairs[0].Value == 0)
            {
                return ParseCoefficientVariablePairList(pairs.Skip(1).ToList(), startingLength);
            }
            else if (pairs[0].Value == 1)
            {
                return $"{beforeIfPositive}{pairs[0].Key}";
            }
            else if (pairs[0].Value == -1)
            {
                return $"-{pairs[0].Key}";
            }
            else if (pairs[0].Value > 0)
            {
                return $"{beforeIfPositive}{pairs[0].Value}{pairs[0].Key}";
            }
            else
            {
                return $"{pairs[0].Value}{pairs[0].Key}";
            }
        }
    }
}
