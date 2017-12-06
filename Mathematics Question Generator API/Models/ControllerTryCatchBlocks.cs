using MathematicsQuestionGeneratorAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public static class ControllerTryCatchBlocks
    {
        public static IActionResult LoggingAllExceptions(ILogger logger, Func<IActionResult> codeBlock, object context = null)
        {
            if (context == null)
            {
                context = new object();
            }
            try
            {
                return codeBlock();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Unexpected error", new object[1] { context });
                throw exception;
            }
        }

        public static IActionResult ReturnBadRequestOnFailedToGenerateExceptionLoggingAllOthers(
            ILogger logger, Func<IActionResult> codeBlock, Func<string, IActionResult> onException, object context = null)
        {
            if (context == null)
            {
                context = new object();
            }
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
                logger.LogError(exception, "Unexpected error", new object[1] { context });
                return onException("Unexpected error.");
            }
        }
    }
}