using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace MathematicsQuestionGeneratorAPI.Models.PdfBuilders
{
    public class BasicPdfBuilder
    {
        private static float MARGIN = 72f;
        private static BaseColor FONT_COLOR = BaseColor.Black;
        private static float FONT_SIZE = 12f;
        private static Font FONT = FontFactory.GetFont(FontFactory.HELVETICA, FONT_SIZE, FONT_COLOR);
        private static int ANSWER_SPACE = 40;
        private static int SPACE_BETWEEN_QUESTIONS = 5;

        public List<string> Questions { get; set; }

        public BasicPdfBuilder(List<string> questions)
        {
            //Questions = questions;

            Questions = new List<string>() { "x^2+x+1=0", "x^2+x+1=0", "5x^3-9x+71=5342", "5x^3-9x+71=5342", "5x^3-9x+71=5342" };
        }

        public void BuildPdf(string saveLocation, string documentName)
        {
            using (var file = File.Create(saveLocation))
            {
                var document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);

                var writer = PdfWriter.GetInstance(document, file);
                document.Open();

                var openingWords = $"Solve the {Questions.Count} quadratic equations below, giving your answers to 2 decimal places." +
                    $"If they are unsolveable, write \"No solution\".";

                var introductoryParagraph = new Paragraph();
                introductoryParagraph.Add(openingWords);
                introductoryParagraph.SpacingAfter = 10;

                document.Add(introductoryParagraph);

                for (var i = 0; i < Questions.Count; i++)
                {
                    // write question
                    var question = new Paragraph();
                    question.SpacingBefore = SPACE_BETWEEN_QUESTIONS;
                    question.SpacingAfter = ANSWER_SPACE;
                    question.Alignment = Element.ALIGN_LEFT;
                    question.Font = FONT;
                    question.Add($"\t\t\t{i + 1}. {Questions[i]}");
                    document.Add(question);

                    // write answer area
                    var answerArea = new Paragraph();
                    answerArea.SpacingBefore = ANSWER_SPACE;
                    answerArea.SpacingAfter = SPACE_BETWEEN_QUESTIONS;
                    answerArea.Alignment = Element.ALIGN_RIGHT;
                    answerArea.Font = FONT;
                    answerArea.Add("Answer....................");
                    document.Add(answerArea);
                }

                document.Close();
            }
        }
    }
}