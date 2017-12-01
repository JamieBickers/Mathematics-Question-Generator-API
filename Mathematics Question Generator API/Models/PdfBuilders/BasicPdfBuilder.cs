using System.Collections.Generic;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace MathematicsQuestionGeneratorAPI.Models.PdfBuilders
{
    public class BasicPdfBuilder
    {
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
            var document = new Document(PageSize.A4, PdfStylings.MARGIN, PdfStylings.MARGIN, PdfStylings.MARGIN, PdfStylings.MARGIN);
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

            var titleParagraph = new Paragraph(title)
            {
                Alignment = Element.ALIGN_RIGHT,
                Font = PdfStylings.FONT_TITLE
            };
            document.Add(titleParagraph);

            var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            var introductoryParagraph = new Paragraph
            {
                instrutions
            };
            introductoryParagraph.SpacingBefore = PdfStylings.SPACE_AFTER_TITLE;
            introductoryParagraph.SpacingAfter = PdfStylings.SPACE_AFTER_INSTRUCTIONS; ;

            document.Add(introductoryParagraph);
        }
    }
}