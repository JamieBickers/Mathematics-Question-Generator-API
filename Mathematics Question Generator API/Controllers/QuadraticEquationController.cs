using Microsoft.AspNetCore.Mvc;
using Mathematics_Questions_Generator.Model;

namespace Mathematics_Question_Generator_API.Controllers
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
