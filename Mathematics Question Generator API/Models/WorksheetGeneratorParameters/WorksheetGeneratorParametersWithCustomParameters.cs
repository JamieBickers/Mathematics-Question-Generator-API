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
        public string EmailAddress;
        public List<QuestionGeneratorParameterType> QuestionGeneratorParameters;

        public WorksheetGeneratorParametersWithCustomParameters(string emailAddress, List<QuestionGeneratorParameterType> questionGeneratorParameters)
        {
            EmailAddress = emailAddress;
            QuestionGeneratorParameters = questionGeneratorParameters;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var parameterErrors = QuestionGeneratorParameters.SelectMany(parameter => parameter.Validate(new ValidationContext(parameter)));
            var attribute = new EmailAddressAttribute();

            foreach (var error in parameterErrors)
            {
                yield return error;
            }

            if (!attribute.IsValid(EmailAddress))
            {
                yield return new ValidationResult("Invalid email address.");
            }
        }
    }
}
