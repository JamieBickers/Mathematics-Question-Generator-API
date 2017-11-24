using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public interface IQuestion
    {
        PdfPCell ParseToPdfPCell(int questionNumber, bool showAnswers);
    }
}
