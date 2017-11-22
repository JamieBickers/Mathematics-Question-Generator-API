using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using Ninject;
using System.Reflection;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/QuadraticEquation")]
    public class QuadraticEquationController : Controller
    {
        private readonly IRandomIntegerGenerator randomIntegerGenerator;

        public QuadraticEquationController()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            randomIntegerGenerator = kernel.Get<IRandomIntegerGenerator>();
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
            parameters.Fill();
            var equationGenerator = new QuadraticEquationGenerator(parameters, randomIntegerGenerator);
            return equationGenerator.GenerateQuestionAndAnswer();
        }
    }
}