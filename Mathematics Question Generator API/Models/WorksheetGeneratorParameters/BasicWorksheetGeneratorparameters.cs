using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.WorksheetGeneratorParameters
{
    public class BasicWorksheetGeneratorparameters : IValidatableObject
    {
        public EmailAddress EmailAddress;
        public int NumberOfQuestions;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return EmailAddress.Validate(new ValidationContext(EmailAddress));
        }
    }
}
