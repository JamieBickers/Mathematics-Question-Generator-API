using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System.Reflection;
using MathematicsQuestionGeneratorAPI.Exceptions;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using MathematicsQuestionGeneratorAPI.Models;

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
            return ControllerTryCatchBlocks.TryCatchLoggingAllExceptions(() =>
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

            return ControllerTryCatchBlocks.TryCatchReturningBadRequestOnFailedToGenerateExceptionLoggingAllOthers(() =>
                {
                    var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator, parameters);
                    return Ok(equationGenerator.GenerateQuestionAndAnswer());
                },
                BadRequest);
        }
    }
}