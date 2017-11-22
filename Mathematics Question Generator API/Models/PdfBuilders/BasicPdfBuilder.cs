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
        public List<string> Questions { get; set; }

        public BasicPdfBuilder(List<string> questions)
        {
            Questions = questions;
        }

        public void BuildPdf(string saveLocation, string documentName)
        {
            using (var file = File.Create(saveLocation))
            {
                var document = new Document(PageSize.A4, 10, 10, 10, 10);

                var writer = PdfWriter.GetInstance(document, file);
                document.Open();

                var chunck = new Chunk("This is a chunck.");
                var phrase = new Phrase("This is a phrase.");
                var para = new Paragraph("This is a paragraph.");

                document.Add(chunck);
                document.Add(phrase);
                document.Add(para);

                var paragraph = new Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.Green);
                paragraph.Add("This is a formatted paragraph.");
                document.Add(paragraph);

                document.Close();
            }
        }
    }
}
