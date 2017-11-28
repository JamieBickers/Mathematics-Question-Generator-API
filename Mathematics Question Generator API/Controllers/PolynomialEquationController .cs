using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.PolynomialEquations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/PolynomialEquation")]
    public class PolynomialEquationController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;

        public PolynomialEquationController(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        // returns a random polynomial equation (with degree given by the url) and its roots
        [HttpGet]
        [Route("{degree}")]
        public IActionResult GetPolynomialEquation(int degree)
        {
            var x = ControllerTryCatchBlocks.TryCatchLoggingAllExceptions(() =>
            {
                var equationGenerator = new PolynomialEquationGenerator(randomIntegerGenerator, new PolynomialEquationGeneratorParameters(degree: degree));
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            });
            return x;
        }

        // return a polynomial equation satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetPolynomialEquation([FromBody] PolynomialEquationGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return ControllerTryCatchBlocks.TryCatchReturningBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
                {
                    var equationGenerator = new PolynomialEquationGenerator(randomIntegerGenerator, parameters);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest);
        }
    }
}