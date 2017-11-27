using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/QuadraticEquation")]
    public class QuadraticEquationController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;

        public QuadraticEquationController(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        // returns a random quadratic equation and its roots
        [HttpGet]
        public IActionResult GetQuadraticEquation()
        {
            var x = ControllerTryCatchBlocks.TryCatchLoggingAllExceptions(() =>
            {
                var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            });
            return x;
        }

        // return a quadratic equation satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetQuadraticEquation([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchReturningBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
                {
                    var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator, parameters);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest);
        }
    }
}