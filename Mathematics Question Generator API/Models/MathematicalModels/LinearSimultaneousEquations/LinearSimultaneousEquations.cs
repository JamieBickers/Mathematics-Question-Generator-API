using iTextSharp.text;
using iTextSharp.text.pdf;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquations : IQuestion
    {
        public List<int> Coefficients;
        public LinearSimultaneousEquationsSolution Solution;

        public LinearSimultaneousEquations(List<int> coefficients, LinearSimultaneousEquationsSolution solution)
        {
            Coefficients = coefficients;
            Solution = solution;
        }

        public string ParseToString()
        {
            var firstParsed = ParseLinearEquationToString(Coefficients.GetRange(0, 3));
            var secondParsed = ParseLinearEquationToString(Coefficients.GetRange(3, 3));

            return $"{firstParsed}\n{secondParsed}=0\nx={Solution.FirstSolution}, y={Solution.SecondSolution}";
        }

        public PdfPCell ParseToPdfPCell(int questionNumber, bool showAnswers)
        {
            var firstParsed = ParseLinearEquationToString(Coefficients.GetRange(0, 3));
            var secondParsed = ParseLinearEquationToString(Coefficients.GetRange(3, 3));

            var firstEquation = new Paragraph($"      {firstParsed}"); // 6 spaces
            firstEquation.Font = PdfStylings.FONT_BODY;

            var secondEquation = new Paragraph($"           {secondParsed}"); // 10 spaces
            firstEquation.Font = PdfStylings.FONT_BODY;

            var question = new Paragraph
            {
                $"{questionNumber.ToString()}.  ",
                firstEquation,
                secondEquation
            };
            question.Font = PdfStylings.FONT_BODY;
            question.SpacingAfter = 100;

            var answerArea = CreateAnswerArea(showAnswers);

            var questionAndAnswer = new PdfPCell();
            questionAndAnswer.AddElement(question);
            questionAndAnswer.AddElement(answerArea);
            questionAndAnswer.BorderColor = BaseColor.White;

            return questionAndAnswer;
        }

        private Paragraph CreateAnswerArea(bool showAnswers)
        {
            var xAnswer = showAnswers ? Math.Round(Solution.FirstSolution, 2).ToString() : "........";
            var yAnswer = showAnswers ? Math.Round(Solution.SecondSolution, 2).ToString() : "........";

            var answerArea = new Paragraph($"Answer: x={xAnswer}, y={yAnswer}")
            {
                Alignment = Element.ALIGN_RIGHT
            };

            answerArea.Font = PdfStylings.FONT_BODY;
            return answerArea;
        }

        public string ParseLinearEquationToString(List<int> coefficients)
        {
            var pairs = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("x", coefficients[0]),
                new KeyValuePair<string, int>("y", coefficients[1]),
                new KeyValuePair<string, int>("", coefficients[2])
            };


            return $"{ParseCoefficientVariablePairList(pairs, pairs.Count)}=0";
        }

        private string ParseCoefficientVariablePairList(List<KeyValuePair<string, int>> pairs, int startingLength)
        {
            if (pairs.Count == 0)
            {
                return "";
            }
            var beforeIfPositive = startingLength == pairs.Count ? "" : "+";
            var rest = ParseCoefficientVariablePairList(pairs.Skip(1).ToList(), startingLength);

            if (pairs[0].Value == 0)
            {
                return rest;
            }
            else if (pairs[0].Value == 1)
            {
                return $"{beforeIfPositive}{pairs[0].Key}{rest}";
            }
            else if (pairs[0].Value == -1)
            {
                return $"-{pairs[0].Key}{rest}";
            }
            else if (pairs[0].Value > 0)
            {
                return $"{beforeIfPositive}{pairs[0].Value}{pairs[0].Key}{rest}";
            }
            else
            {
                return $"{pairs[0].Value}{pairs[0].Key}{rest}";
            }
        }
    }
}
