using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class WorksheetGeneratorParametersWithCustomParameters<QuestionType, QuestionGeneratorParameterType> : IValidatableObject
        where QuestionType : IQuestion
        where QuestionGeneratorParameterType : IValidatableObject
    {
        public EmailAddress EmailAddress;
        public List<QuestionGeneratorParameterType> QuestionGeneratorParameters;

        public WorksheetGeneratorParametersWithCustomParameters(EmailAddress emailAddress, List<QuestionGeneratorParameterType> questionGeneratorParameters)
        {
            EmailAddress = emailAddress;
            QuestionGeneratorParameters = questionGeneratorParameters;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var parameterErrors = QuestionGeneratorParameters.SelectMany(parameter => parameter.Validate(new ValidationContext(parameter)));
            var emailErrors = EmailAddress.Validate(new ValidationContext(EmailAddress));

            foreach (var error in parameterErrors)
            {
                yield return error;
            }

            foreach (var error in emailErrors)
            {
                yield return error;
            }
        }
    }
}