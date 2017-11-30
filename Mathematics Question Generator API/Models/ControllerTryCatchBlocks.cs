using MathematicsQuestionGeneratorAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public static class ControllerTryCatchBlocks
    {
        public static IActionResult LoggingAllExceptions(Func<IActionResult> codeBlock)
        {
            try
            {
                return codeBlock();
            }
            catch (Exception exception)
            {
                //TODO: Logging here
                throw exception;
            }
        }

        public static IActionResult ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(
            Func<IActionResult> codeBlock, Func<string, IActionResult> onException)
        {
            try
            {
                return codeBlock();
            }
            catch (FailedToGenerateQuestionSatisfyingParametersException)
            {
                return onException("Failed to generate question matching those parameters. Tried 1,000,000 times but all failed.");
            }
            catch (Exception exception)
            {
                //TODO: Logging here
                throw exception;
            }
        }
    }
}