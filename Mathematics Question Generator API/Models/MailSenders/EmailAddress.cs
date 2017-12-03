using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{
    public class EmailAddress : IValidatableObject
    {
        public string Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsEmailAddressValid(Address))
            {
                yield return new ValidationResult("Invalid email address.");
            }
        }

        public static bool IsEmailAddressValid(string emailAddress)
        {
            var attribute = new EmailAddressAttribute();
            return attribute.IsValid(emailAddress);
        }
    }
}
