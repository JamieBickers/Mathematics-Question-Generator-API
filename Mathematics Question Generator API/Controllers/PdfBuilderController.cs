using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System.Collections.Generic;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/worksheet")]
    public class PdfBuilderController : Controller
    {     
        [Route("defaultQuadraticEquations")]
        [HttpPost]
        public void GenerateDefaultQuadraticEquationsWorksheet([FromBody] string emailAddress)
        {
            var integerGenerator = new RandomIntegerGenerator();
            var equationGenerator = new QuadraticEquationGenerator(integerGenerator);
            var questions = new List<IQuestion>();
            for (int i = 0; i < 10; i++)
            {
                questions.Add(equationGenerator.GenerateQuestionAndAnswer());
            }
            var pdfBuilder = new BasicPdfBuilder(questions, "title", "instructions");
            var streams = pdfBuilder.CreatePdfsAsMemoryStreams();

            var mailSender = new SmtpMailSender();
            mailSender.SendEmail(emailAddress, streams);

            foreach (var stream in streams)
            {
                stream.Dispose();
            }
        }

        [Route("defaultSimultaneousEquations")]
        [HttpPost]
        public void GenerateDefaultSimultaneousEquationsWorksheet([FromBody] string emailAddress)
        {
            var integerGenerator = new RandomIntegerGenerator();
            var equationGenerator = new LinearSimultaneousEquationsGenerator(integerGenerator);
            var questions = new List<IQuestion>();
            for (int i = 0; i < 10; i++)
            {
                questions.Add(equationGenerator.GenerateQuestionAndAnswer());
            }
            var pdfBuilder = new BasicPdfBuilder(questions, "title", "instructions");
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

            //var pdfBuilder = new BasicPdfBuilder(equationParameters);

            //var streams = pdfBuilder.CreatePdfsAsMemoryStreams();

            var mailSender = new SmtpMailSender();
            //mailSender.SendEmail(worksheetParameters.EmailAddress, streams);

            //foreach (var stream in streams)
            //{
                //stream.Dispose();
            //}
        }
    }
}