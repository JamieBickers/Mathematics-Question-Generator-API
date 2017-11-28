using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System.Collections.Generic;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;
using MathematicsQuestionGeneratorAPI.Models.WorksheetGeneratorParameters;

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
        public IActionResult GenerateDefaultQuadraticEquationsWorksheet([FromBody] BasicWorksheetGeneratorparameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchLoggingAllExceptions(() =>
            {
                IQuestionGenerator<QuadraticEquation> equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
                BuildAndSendPdf(equationGenerator, parameters.EmailAddress, parameters.NumberOfQuestions);
                return Ok(ModelState);
            });
        }

        [Route("defaultSimultaneousEquations")]
        [HttpPost]
        public IActionResult GenerateDefaultSimultaneousEquationsWorksheet([FromBody] BasicWorksheetGeneratorparameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchLoggingAllExceptions(() =>
            {
                IQuestionGenerator<LinearSimultaneousEquations> equationGenerator = new LinearSimultaneousEquationsGenerator(randomIntegerGenerator);
                BuildAndSendPdf(equationGenerator, parameters.EmailAddress, parameters.NumberOfQuestions);
                return Ok(ModelState);
            });
        }

        [Route("specifiedQuadraticEquations")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedQuadraticEquationWorksheet(
            [FromBody] WorksheetGeneratorParametersWithCustomParameters<QuadraticEquation, QuadraticEquationGeneratorParameters> worksheetParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchReturningBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
            {
                Func<QuadraticEquationGeneratorParameters, QuadraticEquationGenerator> quadraticEquationGeneratorConstructor
                    = parameter => new QuadraticEquationGenerator(randomIntegerGenerator, parameter);
                BuildAndSendPdf<QuadraticEquationGenerator, QuadraticEquationGeneratorParameters, QuadraticEquation>(
                    quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress);
                return Ok(ModelState);
            },
            BadRequest);
        }

        [Route("specifiedSimultaneousEquations")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedSimultaneousEquationsWorksheet(
            [FromBody] WorksheetGeneratorParametersWithCustomParameters<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters> worksheetParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchReturningBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
            {
                Func<LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquationsGenerator> quadraticEquationGeneratorConstructor
                    = parameter => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameter);
                BuildAndSendPdf<LinearSimultaneousEquationsGenerator, LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquations>(
                    quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress);
                return Ok(ModelState);
            },
            BadRequest);
        }

        private void BuildAndSendPdf(IQuestionGenerator<IQuestion> questionGenerator, string emailAddress, int numberOfQuestions)
        {
            var questions = new List<IQuestion>();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(questionGenerator.GenerateQuestionAndAnswer());
            }
            EmailWorksheetWithGivenQuestions(emailAddress, questions);
        }

        private void BuildAndSendPdf<GeneratorType, ParameterType, QuestionType>(
            Func<ParameterType, GeneratorType> generatorConstructor, List<ParameterType> parameters, string emailAddress)
            where GeneratorType : IQuestionGenerator<QuestionType>
            where ParameterType : IValidatableObject
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