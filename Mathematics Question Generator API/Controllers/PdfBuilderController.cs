using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using System.Linq;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/worksheet")]
    public class PdfBuilderController : Controller
    {     
        [Route("default")]
        [HttpPost]
        public void GenerateDefaultQuadraticEquationWorksheet([FromBody] string emailAddress)
        {
            var pdfBuilder = new BasicPdfBuilder();
            var streams = pdfBuilder.CreatePdfsAsMemoryStreams();

            var mailSender = new SmtpMailSender();
            mailSender.SendEmail(emailAddress, streams);

            foreach (var stream in streams)
            {
                stream.Dispose();
            }
        }

        [Route("specified")]
        [HttpPost]
        public void GenerateUserSpecifiedQuadraticEquationWorksheet([FromBody] WorksheetGeneratorParameters worksheetParameters)
        {
            var equationParameters = worksheetParameters.QuadraticEquationParameters;
            foreach (var parameter in equationParameters)
            {
                parameter.Fill();
            }

            var pdfBuilder = new BasicPdfBuilder(equationParameters);

            var streams = pdfBuilder.CreatePdfsAsMemoryStreams();

            var mailSender = new SmtpMailSender();
            mailSender.SendEmail(worksheetParameters.EmailAddress, streams);

            foreach (var stream in streams)
            {
                stream.Dispose();
            }
        }
    }
}