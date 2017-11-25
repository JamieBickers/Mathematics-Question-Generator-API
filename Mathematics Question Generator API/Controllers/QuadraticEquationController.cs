using Microsoft.AspNetCore.Mvc;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using System.Reflection;
using MathematicsQuestionGeneratorAPI.Exceptions;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
            try
            {
                var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            }
            catch (Exception exception)
            {
                //TODO: Logging here
                throw exception;
            }
        }

        // return a quadratic equation satisfying the user entered parameters
        [HttpPost]
        public IActionResult GetQuadraticEquation([FromBody] QuadraticEquationGeneratorParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var equationGenerator = new QuadraticEquationGenerator(randomIntegerGenerator, parameters);
                return Ok(equationGenerator.GenerateQuestionAndAnswer());
            }
            catch (FailedToGenerateQuestionSatisfyingParametersException)
            {
                return BadRequest("Failed to generate question matching those parameters. Tried 1,000,000 times but all failed.");
            }
            catch (Exception exception)
            {
                //TODO: Logging here
                throw exception;
            }
        }
    }
}