using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")] //TODO: What does this do?
    [Route("api/PdfBuilder")]
    public class PdfBuilderController : Controller
    {     
        // POST: api/PdfBuilder
        [HttpGet]
        public void Post()
        {
            var pdfBuilder = new BasicPdfBuilder(new List<QuadraticEquation>());
            pdfBuilder.BuildPdf(@"C:\Users\Jamie\Desktop\MathematicsQuestionGeneratorAPI\testFile.pdf", "testFile.txt");
        }
    }
}
