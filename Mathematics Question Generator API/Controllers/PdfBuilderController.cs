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
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/worksheet")]
    public class PdfBuilderController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private readonly IMailSender mailSender;
        private readonly QuestionGeneratorContext context;
        private readonly DatabaseQueries queries;
        private readonly ILogger logger;

        public PdfBuilderController(IRandomIntegerGenerator randomIntegerGenerator, IMailSender mailSender,
            QuestionGeneratorContext context, ILogger logger)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.mailSender = mailSender;
            this.context = context;
            queries = new DatabaseQueries(context);
            this.logger = logger;
        }

        [Route("defaultQuadraticEquations")]
        [HttpPost]
        public IActionResult GenerateDefaultQuadraticEquationsWorksheet([FromBody] BasicWorksheetGeneratorparameters parameters)
        {
            return GenerateDefaultWorksheet(parameters, new QuadraticEquationGenerator(randomIntegerGenerator));
        }

        [Route("defaultSimultaneousEquations")]
        [HttpPost]
        public IActionResult GenerateDefaultSimultaneousEquationsWorksheet([FromBody] BasicWorksheetGeneratorparameters parameters)
        {
            return GenerateDefaultWorksheet(parameters, new LinearSimultaneousEquationsGenerator(randomIntegerGenerator));
        }

        [Route("specifiedQuadraticEquations")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedQuadraticEquationWorksheet(
            [FromBody] WorksheetGeneratorParametersWithCustomParameters<QuadraticEquation, QuadraticEquationGeneratorParameters> worksheetParameters)
        {
            Func<QuadraticEquationGeneratorParameters, QuadraticEquationGenerator> generatorConstructor =
                parameters => new QuadraticEquationGenerator(randomIntegerGenerator, parameters);

            return GenerateuserSpecifiedWorksheet(worksheetParameters, generatorConstructor);
        }

        [Route("specifiedSimultaneousEquations")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedSimultaneousEquationsWorksheet(
            [FromBody] WorksheetGeneratorParametersWithCustomParameters<LinearSimultaneousEquations, LinearSimultaneousEquationsGeneratorParameters> worksheetParameters)
        {
            Func<LinearSimultaneousEquationsGeneratorParameters, LinearSimultaneousEquationsGenerator> generatorConstructor =
                parameters => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameters);

            return GenerateuserSpecifiedWorksheet(worksheetParameters, generatorConstructor);
        }

        [Route("mixed")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedMixedWorksheet([FromBody] MixedWorksheetParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(logger, () =>
            {
                var questionGenerators = new List<IQuestionGenerator<IQuestion>>();
                foreach (var parameter in parameters.Parameters)
                {
                    if (parameter.QuestionType == "quadratic")
                    {
                        var serialisedParameter = JsonConvert.SerializeObject(parameter.Parameter);
                        var quadraticParameter = JsonConvert.DeserializeObject(serialisedParameter, typeof(QuadraticEquationGeneratorParameters));

                        questionGenerators.Add(new QuadraticEquationGenerator(
                            randomIntegerGenerator, (QuadraticEquationGeneratorParameters)quadraticParameter));
                    }
                    else if (parameter.QuestionType == "simultaneous")
                    {
                        var serialisedParameter = JsonConvert.SerializeObject(parameter.Parameter);
                        var simultaneousParameter = JsonConvert.DeserializeObject(serialisedParameter, typeof(LinearSimultaneousEquationsGeneratorParameters));

                        questionGenerators.Add(new LinearSimultaneousEquationsGenerator(
                            randomIntegerGenerator, (LinearSimultaneousEquationsGeneratorParameters)simultaneousParameter));
                    }
                }
                BuildAndSendPdf(questionGenerators, parameters.EmailAddress.Address);
                return Ok(ModelState);
            },
            BadRequest, parameters);
        }

        [Route("allPreviousWorksheets")]
        [HttpPost]
        public IActionResult GenerateAllPreviouslySentWorksheets([FromBody] EmailAddress emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!queries.CheckIfUserIsInDatabase(emailAddress.Address))
            {
                return BadRequest("This email address has not been used before.");
            }
            var worksheets = queries.SelectAllWorksheetsByUser(emailAddress.Address);

            foreach (var worksheet in worksheets)
            {
                EmailWorksheetWithGivenQuestionsAsync(emailAddress.Address, worksheet);
            }
            return Ok();
        }

        private IActionResult GenerateDefaultWorksheet(BasicWorksheetGeneratorparameters parameters, IQuestionGenerator<IQuestion> generator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.LoggingAllExceptions(logger, () =>
            {
                var generators = Enumerable.Range(0, parameters.NumberOfQuestions)
                .Select(t => generator)
                .ToList();

                BuildAndSendPdf(generators, parameters.EmailAddress.Address);
                return Ok(ModelState);
            }, new { parameters, generator });
        }

        private IActionResult GenerateuserSpecifiedWorksheet<TGeneratorParamaters, TQuestion>(
            WorksheetGeneratorParametersWithCustomParameters<TQuestion, TGeneratorParamaters> worksheetParameters,
            Func<TGeneratorParamaters, IQuestionGenerator<IQuestion>> generatorConstructor)
            where TGeneratorParamaters : IValidatableObject
            where TQuestion : IQuestion
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(logger, () =>
            {
                var generators =
                worksheetParameters.QuestionGeneratorParameters
                    .Select(parameter => generatorConstructor(parameter))
                    .ToList();

                BuildAndSendPdf(generators, worksheetParameters.EmailAddress.Address);
                return Ok(ModelState);
            },
            BadRequest, worksheetParameters);
        }

        private async void BuildAndSendPdf(List<IQuestionGenerator<IQuestion>> questionGenerators, string emailAddress, bool addToDatabase = true)
        {
            var questions = questionGenerators.Select(generator => generator.GenerateQuestionAndAnswer()).ToList();

            if (addToDatabase)
            {
                await AddToDatabase(emailAddress, questions);
            }

            EmailWorksheetWithGivenQuestionsAsync(emailAddress, questions);
        }

        private void EmailWorksheetWithGivenQuestionsAsync(string emailAddress, List<IQuestion> questions)
        {
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
                queries.InsertWorksheet(questions, emailAddress);
            }
            catch (Exception exception)
            {
                logger.LogError("Error adding worksheet to database.", new object[] { exception, emailAddress, questions });
            }

            return Task.Delay(0);
        }
    }
}