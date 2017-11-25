using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System.Reflection;

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
        public QuadraticEquation GetQuadraticEquation()
        {
            var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
            return equationGenerator.GenerateQuestionAndAnswer();
        }

        // return a quadratic equation satisfying the user entered parameters
        [HttpPost]
        public QuadraticEquation GetQuadraticEquation([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator, parameters);
            return equationGenerator.GenerateQuestionAndAnswer();
        }
    }
}