using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.WorksheetGeneratorParameters
{
    public class BasicWorksheetGeneratorparameters : IValidatableObject
    {
        public string EmailAddress;
        public int NumberOfQuestions;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var attribute = new EmailAddressAttribute();

            if (!attribute.IsValid(EmailAddress))
            {
                yield return new ValidationResult("Invalid email address.");
            }
        }
    }
}
