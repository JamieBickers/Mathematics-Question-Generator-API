using iTextSharp.text;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquation : IQuestion
    {
        public List<int> Coefficients;
        public List<double> Roots;

        public QuadraticEquation(List<int> coefficients, List<double> roots)
        {
            Coefficients = coefficients;
            Roots = roots;
        }

        public string ParseToString()
        {
            int a = Coefficients[0];
            int b = Coefficients[1];
            int c = Coefficients[2];
            double root1 = Roots[0];
            double root2 = Roots[1];

            string aTerm = (a == 1) ? "" : $"{a}";
            string bTerm = (b < 0) ? $"{b}" : $"+{b}";
            string cTerm = (c < 0) ? $"{c}" : $"+{c}";

            return $"Question: {aTerm}x^2{bTerm}x{cTerm}=0\nRoots: {root1.ToString()}, {root2.ToString()}";
        }

        public PdfPCell ParseToPdfPCell(int questionNumber, bool showAnswers)
        {
            var question = CreateQuestionParagraph(questionNumber);

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
            string answers = showAnswers ? $"{DisplayActualAnswers()}" : "................."; ;

            var answerArea = new Paragraph($"Answer: {answers}")
            {
                Alignment = Element.ALIGN_RIGHT
            };
            answerArea.Font = PdfStylings.FONT_BODY;

            return answerArea;
        }

        private string DisplayActualAnswers()
        {
            if (Roots.Exists(root => Double.IsNaN(root)))
            {
                return "no solution";
            }
            else if (Math.Abs(Roots[0] - Roots[1]) < Math.Abs(Roots[0] * 0.0001))
            {
                return Math.Round(Roots[0], 2).ToString();
            }
            else
            {
                return $"{Math.Round(Roots[0], 2).ToString()}, {Math.Round(Roots[1], 2).ToString()}";
            }
        }

        private Paragraph CreateQuestionParagraph(int questionNumber)
        {
            int a = Coefficients[0];
            int b = Coefficients[1];
            int c = Coefficients[2];

            var equation = new Paragraph($"{questionNumber}.    {ConvertLeadingTermToString()}x"); // 4 spaces

            var power = new Chunk("2");
            power.SetTextRise(7);
            equation.Add(power);

            equation.Add(ConvertBTermToString());
            equation.Add(ConvertCTermToString());
            equation.Add("=0");

            equation.Font = PdfStylings.FONT_BODY;
            return equation;
        }

        private string ConvertCTermToString()
        {
            var c = Coefficients[2];
            if (c == 0)
            {
                return "";
            }
            else if (c == -1)
            {
                return "-x";
            }
            else if (c < 0)
            {
                return $"{c.ToString()}";
            }
            else
            {
                return $"+{c.ToString()}";
            }
        }

        private string ConvertBTermToString()
        {
            var b = Coefficients[1];
            if (b == 0)
            {
                return "";
            }
            else if (b == 1)
            {
                return "+x";
            }
            else if (b == -1)
            {
                return "-x";
            }
            else if (b < 0)
            {
                return $"{b.ToString()}";
            }
            else
            {
                return $"+{b.ToString()}x";
            }
        }

        private string ConvertLeadingTermToString()
        {
            int a = Coefficients[0];
            if (a == 0)
            {
                throw new Exception("a term cannot be 0.");
            }
            else if (a == 1)
            {
                return "";
            }
            else if (a == -1)
            {
                return "-";
            }
            else
            {
                return a.ToString();
            }
        }
    }
}