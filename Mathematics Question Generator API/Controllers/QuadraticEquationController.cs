using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Route("api/QuadraticEquation")]
    public class QuadraticEquationController : Controller
    {
        // returns a random quadratic equation and its roots
        [HttpGet]
        public QuadraticEquation GetQuadraticEquation()
        {
            var equationGenerator = new QuadraticEquationGenerator();
            return equationGenerator.GenerateQuestionAndAnswer();
        }

        [HttpPost]
        public QuadraticEquation GetQuadraticEquation([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            var equationGenerator = new QuadraticEquationGenerator(parameters);
            return equationGenerator.GenerateQuestionAndAnswer();
        }
    }
}