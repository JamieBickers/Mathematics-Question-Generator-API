using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/simultaneousequations")]
    public class LinearSimultaneousEquationsController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;

        public LinearSimultaneousEquationsController(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        // returns random simultaneous equations and its solutions
        [HttpGet]
        public IActionResult GetLinearSimultaneousEquations()
        {
            return ControllerTryCatchBlocks.LoggingAllExceptions(() =>
            {
                var equationGenerator = new LinearSimultaneousEquationsGenerator(randomIntegerGenerator);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            });
        }

        // return simultaneous equations equation satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetLinearSimultaneousEquations([FromBody] LinearSimultaneousEquationsGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
                {
                    var equationGenerator = new LinearSimultaneousEquationsGenerator(randomIntegerGenerator, parameters);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest);
        }
    }
}