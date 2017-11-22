using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models.Wrappers;
using System.Linq;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")] //TODO: What does this do?
    [Route("api/PdfBuilder")]
    public class PdfBuilderController : Controller
    {     
        [HttpGet]
        public void GenerateDefaultQuadraticEquationWorksheet()
        {
            var pdfBuilder = new BasicPdfBuilder();
            pdfBuilder.BuildPdf(@"C:\Users\Jamie\Desktop\MathematicsQuestionGeneratorAPI\testFile.pdf");
        }

        [HttpPost]
        public void GenerateUserSpecifiedQuadraticEquationWorksheet([FromBody] QuadraticEquationGeneratorParamaterListWrapper parametersWrapper)
        {
            var parameters = parametersWrapper.parameters;
            foreach (var parameter in parameters)
            {
                parameter.Fill();
            }
            var pdfBuilder = new BasicPdfBuilder(parameters);
            pdfBuilder.BuildPdf(@"C:\Users\Jamie\Desktop\MathematicsQuestionGeneratorAPI\testFile.pdf");
        }
    }
}
