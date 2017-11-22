using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;

namespace MathematicsQuestionGeneratorAPI.Models.PdfBuilders
{
    public class BasicPdfBuilder
    {
        private static float MARGIN = 72f;
        private static BaseColor FONT_COLOR = BaseColor.Black;
        private static float FONT_SIZE_BODY = 12f;
        private static float FONT_SIZE_TITLE = 20f;
        private static Font FONT_BODY = FontFactory.GetFont(FontFactory.HELVETICA, FONT_SIZE_BODY, FONT_COLOR);
        private static Font FONT_TITLE = FontFactory.GetFont(FontFactory.HELVETICA, FONT_SIZE_TITLE, FONT_COLOR);
        private static int ANSWER_SPACE = 34;
        private static int SPACE_BETWEEN_QUESTIONS = 5;
        private static int SPACE_AFTER_TITLE = 18;

        public List<QuadraticEquation> Questions { get; set; }

        public BasicPdfBuilder()
        {
            var integerGenerator = new RandomIntegerGenerator();
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator);
            Questions = new List<QuadraticEquation>();

            for (int i = 0; i < 5; i++)
            {
                Questions.Add(equationGenerator.GenerateQuestionAndAnswer());
            }
        }

        public BasicPdfBuilder(List<QuadraticEquationGeneratorParameters> parameters)
        {
            Questions = parameters.Select(parameter => (new QuadraticEquationGenerator(parameter, new RandomIntegerGenerator())).GenerateQuestionAndAnswer()).ToList();
        }

        public void BuildPdf(string saveLocation)
        {
            using (var file = File.Create(saveLocation))
            {
                var document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);

                var writer = PdfWriter.GetInstance(document, file);
                document.Open();

                var titleWords = "Quadratic Equations";

                var title = new Paragraph(titleWords);
                title.Alignment = Element.ALIGN_RIGHT;
                title.Font = FONT_TITLE;
                document.Add(title);

                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
                document.Add(line);

                var openingWords = $"Solve the {Questions.Count} quadratic equations below, giving your answers to 2 decimal places." +
                    $"If they are unsolveable, write \"no solution\".";

                var introductoryParagraph = new Paragraph();
                introductoryParagraph.Add(openingWords);
                introductoryParagraph.SpacingBefore = SPACE_AFTER_TITLE;
                introductoryParagraph.SpacingAfter = 10;

                document.Add(introductoryParagraph);

                for (var i = 0; i < Questions.Count; i++)
                {
                    WriteSingleQuestionAndAnswerToDocument(document, Questions[i], i, false);
                }

                document.Close();
            }
        }

        private void WriteSingleQuestionAndAnswerToDocument(Document document, QuadraticEquation equation, int questionNumber, bool ShowAnswer)
        {
            var a = Questions[questionNumber].Coefficients["a"];
            var b = Questions[questionNumber].Coefficients["b"];
            var c = Questions[questionNumber].Coefficients["c"];

            // write question
            var question = new Paragraph();
            question.SpacingBefore = SPACE_BETWEEN_QUESTIONS;
            question.SpacingAfter = ANSWER_SPACE;
            question.Alignment = Element.ALIGN_LEFT;
            question.Font = FONT_BODY;
            question.Add($"\t\t\t{questionNumber + 1}. ");
            question.Add(QuadraticEquationParser.ParseToPdfParagraph(a, b, c));
            document.Add(question);

            // write answer area
            var answerArea = new Paragraph();
            answerArea.SpacingBefore = ANSWER_SPACE;
            answerArea.SpacingAfter = SPACE_BETWEEN_QUESTIONS;
            answerArea.Alignment = Element.ALIGN_RIGHT;
            answerArea.Font = FONT_BODY;
            answerArea.Add("Answer....................");
            document.Add(answerArea);
        }
    }
}