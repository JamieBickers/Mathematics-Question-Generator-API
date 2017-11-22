using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;

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
            var pdfBuilder = new BasicPdfBuilder(new List<string>());
            pdfBuilder.BuildPdf(@"C:\Users\Jamie\Desktop\MathematicsQuestionGeneratorAPI\testFile.pdf", "testFile.txt");
        }
    }
}
