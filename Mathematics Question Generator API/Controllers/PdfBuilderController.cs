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
using MathematicsQuestionGeneratorAPI.Models.WorksheetGeneratorParameters;
using MathematicsQuestionGeneratorAPI.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/worksheet")]
    public class PdfBuilderController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private readonly IMailSender mailSender;
        private readonly QuestionGeneratorContext context;
        private readonly DatabaseQueries Queries;

        public PdfBuilderController(IRandomIntegerGenerator randomIntegerGenerator, IMailSender mailSender, QuestionGeneratorContext context)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.mailSender = mailSender;
            this.context = context;
            this.Queries = new DatabaseQueries(context);
        }

        [Route("defaultQuadraticEquations")]
        [HttpPost]
        public IActionResult GenerateDefaultQuadraticEquationsWorksheet([FromBody] BasicWorksheetGeneratorparameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.LoggingAllExceptions(() =>
            {
                IQuestionGenerator<QuadraticEquation> equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
                BuildAndSendPdf(equationGenerator, parameters.EmailAddress.Address, parameters.NumberOfQuestions);
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

            return ControllerTryCatchBlocks.LoggingAllExceptions(() =>
            {
                IQuestionGenerator<LinearSimultaneousEquations> equationGenerator = new LinearSimultaneousEquationsGenerator(randomIntegerGenerator);
                BuildAndSendPdf(equationGenerator, parameters.EmailAddress.Address, parameters.NumberOfQuestions);
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

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
            {
                Func<QuadraticEquationGeneratorParameters, QuadraticEquationGenerator> quadraticEquationGeneratorConstructor
                    = parameter => new QuadraticEquationGenerator(randomIntegerGenerator, parameter);
                BuildAndSendPdf<QuadraticEquationGenerator, QuadraticEquationGeneratorParameters, QuadraticEquation>(
                    quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress.Address);
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

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
            {
                Func<LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquationsGenerator> quadraticEquationGeneratorConstructor
                    = parameter => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameter);
                BuildAndSendPdf<LinearSimultaneousEquationsGenerator, LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquations>(
                    quadraticEquationGeneratorConstructor, worksheetParameters.QuestionGeneratorParameters, worksheetParameters.EmailAddress.Address);
                return Ok(ModelState);
            },
            BadRequest);
        }

        [Route("allPreviousWorksheets")]
        [HttpPost]
        public IActionResult GenerateAllPreviouslySentWorksheets([FromBody] EmailAddress emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var worksheets = Queries.SelectAllWorksheetsByUser(emailAddress.Address);

            foreach (var worksheet in worksheets)
            {
                EmailWorksheetWithGivenQuestionsAsync(emailAddress.Address, worksheet, false);
            }
            return Ok();
        }

        private void BuildAndSendPdf(IQuestionGenerator<IQuestion> questionGenerator, string emailAddress,
            int numberOfQuestions, bool addToDatabase = true)
        {
            var questions = Enumerable.Range(0, numberOfQuestions)
                .Select(x => questionGenerator.GenerateQuestionAndAnswer())
                .ToList();
            EmailWorksheetWithGivenQuestionsAsync(emailAddress, questions, addToDatabase);
        }

        private void BuildAndSendPdf<GeneratorType, ParameterType, QuestionType>(
            Func<ParameterType, GeneratorType> generatorConstructor, List<ParameterType> parameters,
            string emailAddress, bool addToDatabase = true)

            where GeneratorType : IQuestionGenerator<QuestionType>
            where ParameterType : IValidatableObject
            where QuestionType : IQuestion
        {
            var questions = new List<IQuestion>();
            for (var i = 0; i < parameters.Count; i++)
            {
                var generator = generatorConstructor(parameters[i]);
                questions.Add(generator.GenerateQuestionAndAnswer());
            }
            EmailWorksheetWithGivenQuestionsAsync(emailAddress, questions, addToDatabase);
        }

        private async void EmailWorksheetWithGivenQuestionsAsync(string emailAddress, List<IQuestion> questions, bool addToDatabase)
        {
            if (addToDatabase)
            {
                await AddToDatabase(emailAddress, questions);
            }

            var pdfBuilder = new BasicPdfBuilder(questions, "title", "instructions");
            var streams = pdfBuilder.CreatePdfsAsMemoryStreams();

            mailSender.SendEmail(emailAddress, streams);

            foreach (var stream in streams)
            {
                stream.Dispose();
            }
        }

        private Task AddToDatabase(string emailAddress, List<IQuestion> questions)
        {
            try
            {
                var type = questions.First().GetType();

                if (type == typeof(QuadraticEquation))
                {
                    var quadraticQuestions = questions.Select(question => (QuadraticEquation)question).ToList();
                    Queries.InsertQuadraticWorksheet(quadraticQuestions, emailAddress);
                }
                else if (type == typeof(LinearSimultaneousEquations))
                {
                    var simultaneousQuestions = questions.Select(question => (LinearSimultaneousEquations)question).ToList();
                    Queries.InsertSimultaneousWorksheet(simultaneousQuestions, emailAddress);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch
            {
                //TODO: Logging
            }

            return Task.Delay(0);
        }
    }
}