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

        [Route("specifiedQuadraticEquations")]
        [HttpPost]
        public void GenerateUserSpecifiedQuadraticEquationWorksheet(
            [FromBody] WorksheetGeneratorParameters<QuadraticEquation, QuadraticEquationGeneratorParameters> worksheetParameters)
        {
            Func<QuadraticEquationGeneratorParameters, QuadraticEquationGenerator> quadraticEquationGeneratorConstructor
                = parameter => new QuadraticEquationGenerator(parameter, randomIntegerGenerator);
            BuildAndSendPdf<QuadraticEquationGenerator, QuadraticEquationGeneratorParameters, QuadraticEquation>(
                quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress);
        }

        [Route("specifiedSimultaneousEquations")]
        [HttpPost]
        public void GenerateUserSpecifiedSimultaneousEquationsWorksheet(
            [FromBody] WorksheetGeneratorParameters<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters> worksheetParameters)
        {
            Func<LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquationsGenerator> quadraticEquationGeneratorConstructor
                = parameter => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameter);
            BuildAndSendPdf<LinearSimultaneousEquationsGenerator, LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquations>(
                quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress);
        }

        private void BuildAndSendPdf(IQuestionGenerator<IQuestion> questionGenerator, string emailAddress)
        {
            var questions = new List<IQuestion>();
            for (int i = 0; i < 10; i++)
            {
                questions.Add(questionGenerator.GenerateQuestionAndAnswer());
            }
            EmailWorksheetWithGivenQuestions(emailAddress, questions);
        }

        private void BuildAndSendPdf<GeneratorType, ParameterType, QuestionType>(
            Func<ParameterType, GeneratorType> generatorConstructor, List<ParameterType> parameters, string emailAddress)
            where GeneratorType : IQuestionGenerator<QuestionType>
            where ParameterType : QuestionParameters
            where QuestionType : IQuestion
        {
            var questions = new List<IQuestion>();
            for (int i = 0; i < parameters.Count; i++)
            {
                var generator = generatorConstructor(parameters[i]);
                questions.Add(generator.GenerateQuestionAndAnswer());
            }
            EmailWorksheetWithGivenQuestions(emailAddress, questions);
        }

        private static void EmailWorksheetWithGivenQuestions(string emailAddress, List<IQuestion> questions)
        {
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