using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System.Collections.Generic;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using System;
using System.Reflection;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/worksheet")]
    public class PdfBuilderController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;

        public PdfBuilderController(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        [Route("defaultQuadraticEquations")]
        [HttpPost]
        public void GenerateDefaultQuadraticEquationsWorksheet([FromBody] string emailAddress)
        {
            IQuestionGenerator<QuadraticEquation> equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
            BuildAndSendPdf(equationGenerator, emailAddress);
        }

        [Route("defaultSimultaneousEquations")]
        [HttpPost]
        public void GenerateDefaultSimultaneousEquationsWorksheet([FromBody] string emailAddress)
        {
            IQuestionGenerator<LinearSimultaneousEquations> equationGenerator = new LinearSimultaneousEquationsGenerator(randomIntegerGenerator);
            BuildAndSendPdf(equationGenerator, emailAddress);
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

        private void BuildAndSendPdf(IQuestionGenerator<IQuestion> questionGenerator, string emailAddress)
        {
            var questions = new List<IQuestion>();
            for (int i = 0; i < 10; i++)
            {
                questions.Add(questionGenerator.GenerateQuestionAndAnswer());
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
    }
}