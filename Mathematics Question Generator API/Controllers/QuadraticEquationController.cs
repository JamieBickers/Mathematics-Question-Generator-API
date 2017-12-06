using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models;
using Microsoft.Extensions.Logging;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/QuadraticEquation")]
    public class QuadraticEquationController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;
        private readonly ILogger logger;

        public QuadraticEquationController(IRandomIntegerGenerator randomIntegerGenerator, ILogger<QuadraticEquationController> logger)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
            this.logger = logger;
        }

        // returns a random quadratic equation and its roots
        [HttpGet]
        public IActionResult GetQuadraticEquation()
        {
             return ControllerTryCatchBlocks.LoggingAllExceptions(logger, () =>
            {
                var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            });
        }

        // return a quadratic equation satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetQuadraticEquation([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(logger, () =>
                {
                    var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator, parameters);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest, parameters);
        }
    }
}