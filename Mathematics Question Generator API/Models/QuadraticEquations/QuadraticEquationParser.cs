using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public static class QuadraticEquationParser
    {
        public static string ParseToString(int a, int b, int c, double root1, double root2)
        {
            string aTerm = (a == 1) ? "" : $"{a}";
            string bTerm = (b < 0) ? $"{b}" : $"+{b}";
            string cTerm = (c < 0) ? $"{c}" : $"+{c}";

            return $"Question: {aTerm}x^2{bTerm}x{cTerm}=0\nRoots: {root1.ToString()}, {root2.ToString()}";
        }

        public static Paragraph ParseToPdfParagraph(int a, int b, int c)
        {
            string aTerm = (a == 1) ? "" : $"{a}";
            string bTerm = (b < 0) ? $"{b}" : $"+{b}";
            string cTerm = (c < 0) ? $"{c}" : $"+{c}";

            Paragraph equation = new Paragraph();

            Chunk beforeSquare = new Chunk($"{aTerm}x");
            Chunk square = new Chunk("2");
            square.SetTextRise(5);
            Chunk afterSquare = new Chunk($"{bTerm}x{cTerm}=0");

            equation.Add(beforeSquare);
            equation.Add(square);
            equation.Add(afterSquare);

            return equation;
        }
    }
}
