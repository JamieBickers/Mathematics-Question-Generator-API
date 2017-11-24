using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System;

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

        private List<IQuestion> questions;
        private string title;
        private string instrutions;

        public BasicPdfBuilder(List<IQuestion> questions, string title, string instrutions)
        {
            this.questions = questions;
            this.title = title;
            this.instrutions = instrutions;
        }

        public void CreatePdfsAndSaveLocallyAsFiles(string saveLocation)
        {
            using (var questionSheet = File.Create(saveLocation + "Question Sheet.pdf"))
            using (var answerSheet = File.Create(saveLocation + "Answer Sheet.pdf"))
            {
                WriteDocumentToGivenStream(questionSheet, false);
                WriteDocumentToGivenStream(answerSheet, true);
            }
        }

        public List<MemoryStream> CreatePdfsAsMemoryStreams()
        {
            var questionSheet = new MemoryStream();
            var answerSheet = new MemoryStream();
            WriteDocumentToGivenStream(questionSheet, false);
            WriteDocumentToGivenStream(answerSheet, true);

            return new List<MemoryStream>() { questionSheet, answerSheet };
        }

        private void WriteDocumentToGivenStream(Stream stream, bool displayAnswers)
        {
            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            WriteTitleAndInstructions(stream, document);

            var table = new PdfPTable(1);

            for (var i = 0; i < questions.Count; i++)
            {
                table.AddCell(questions[i].ParseToPdfPCell(i + 1, displayAnswers));
            }

            table.WidthPercentage = 100;
            document.Add(table);
            document.Close();
        }

        private void WriteTitleAndInstructions(Stream stream, Document document)
        {
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            Paragraph introductoryParagraph;
            var titleParagraph = new Paragraph(title);
            titleParagraph.Alignment = Element.ALIGN_RIGHT;
            titleParagraph.Font = FONT_TITLE;
            document.Add(titleParagraph);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            introductoryParagraph = new Paragraph();
            introductoryParagraph.Add(instrutions);
            introductoryParagraph.SpacingBefore = SPACE_AFTER_TITLE;
            introductoryParagraph.SpacingAfter = 10;

            document.Add(introductoryParagraph);
        }
    }
}