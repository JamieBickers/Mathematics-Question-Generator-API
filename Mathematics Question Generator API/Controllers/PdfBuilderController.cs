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
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using System.Collections;
using Newtonsoft.Json;

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
            Queries = new DatabaseQueries(context);
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
                var generators = Enumerable.Range(0, parameters.NumberOfQuestions)
                .Select(x => new QuadraticEquationGenerator(randomIntegerGenerator))
                .ToList<IQuestionGenerator<IQuestion>>();
                BuildAndSendPdf(generators, parameters.EmailAddress.Address);
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
                var generators = Enumerable.Range(0, parameters.NumberOfQuestions)
                .Select(x => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator))
                .ToList<IQuestionGenerator<IQuestion>>();
                BuildAndSendPdf(generators, parameters.EmailAddress.Address);
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
                var generators =
                worksheetParameters.QuestionGeneratorParameters
                    .Select(parameter => new QuadraticEquationGenerator(randomIntegerGenerator, parameter))
                    .ToList<IQuestionGenerator<IQuestion>>();

                BuildAndSendPdf(generators, worksheetParameters.EmailAddress.Address);
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
                var generators =
                worksheetParameters.QuestionGeneratorParameters
                    .Select(parameter => new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameter))
                    .ToList<IQuestionGenerator<IQuestion>>();

                BuildAndSendPdf(generators, worksheetParameters.EmailAddress.Address);
                return Ok(ModelState);
            },
            BadRequest);
        }

        [Route("mixed")]
        [HttpPost]
        public IActionResult GenerateUserSpecifiedMixedWorksheet([FromBody] MixedWorksheetParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
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

            if (!Queries.CheckIfUserIsInDatabase(emailAddress.Address))
            {
                return BadRequest("This email address has not been used before.");
            }
            var worksheets = Queries.SelectAllWorksheetsByUser(emailAddress.Address);

            foreach (var worksheet in worksheets)
            {
                EmailWorksheetWithGivenQuestionsAsync(emailAddress.Address, worksheet, true);
            }
            return Ok();
        }

        private void BuildAndSendPdf(List<IQuestionGenerator<IQuestion>> questionGenerators, string emailAddress, bool addToDatabase = true)
        {
            //TODO; implement various questions on adding to database
            var questions = questionGenerators.Select(generator => generator.GenerateQuestionAndAnswer()).ToList();
            AddToDatabase(emailAddress, questions);
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
                Queries.InsertWorksheet(questions, emailAddress);
            }
            catch
            {
                //TODO: Logging
            }

            return Task.Delay(0);
        }
    }
}