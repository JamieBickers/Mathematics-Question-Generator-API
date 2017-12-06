using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models;
using Microsoft.AspNetCore.Cors;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using System;
using Microsoft.Extensions.Logging;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    //TODO: Properly implement this once I can sort out overriding the routing.
    [Produces("application/json")]
    [Route("")]
    public abstract class QuestionController<TQuestion, TQuestionGenerator> : Controller
        where TQuestion : IQuestion
        where TQuestionGenerator : IQuestionGenerator<TQuestion>
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private readonly Func<IRandomIntegerGenerator, TQuestionGenerator> questionGeneratorConstructor;
        private readonly ILogger logger;

        public QuestionController(IRandomIntegerGenerator randomIntegerGenerator, Func<IRandomIntegerGenerator,
            TQuestionGenerator> questionGeneratorConstructor, ILogger logger)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.questionGeneratorConstructor = questionGeneratorConstructor;
            this.logger = logger;
        }

        // returns a random question and its solution
        [HttpGet]
        public IActionResult GetQuestion()
        {
            return ControllerTryCatchBlocks.LoggingAllExceptions(logger, () =>
            {
                var equationGenerator = questionGeneratorConstructor(randomIntegerGenerator);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            });
        }

        // return a question satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetQuestion([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(logger, () =>
                {
                    var equationGenerator = questionGeneratorConstructor(randomIntegerGenerator);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest);
        }
    }
}