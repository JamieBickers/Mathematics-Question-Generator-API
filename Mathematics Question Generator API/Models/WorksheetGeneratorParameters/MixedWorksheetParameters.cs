using MathematicsQuestionGeneratorAPI.Models;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathematicsQuestionGeneratorAPI.Controllers
{
    public class MixedWorksheetParameters : IValidatableObject
    {
        public List<ParameterWithType> Parameters;
        public EmailAddress EmailAddress;

        public MixedWorksheetParameters(List<ParameterWithType> parameters, EmailAddress emailAddress)
        {
            Parameters = parameters;
            EmailAddress = emailAddress;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            foreach (var parameter in Parameters)
            {
                errors.AddRange(parameter.Validate(new ValidationContext(parameter)));
            }

            errors.AddRange(EmailAddress.Validate(new ValidationContext(EmailAddress)));
            return errors;
        }
    }

    public class ParameterWithType : IValidatableObject
    {
        public object Parameter;
        public string QuestionType;

        public ParameterWithType(object parameter, string questionType)
        {
            Parameter = parameter;
            QuestionType = questionType;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QuestionType == "quadratic")
            {
                var serialisedParameter = JsonConvert.SerializeObject(Parameter);
                var quadraticParameter = JsonConvert.DeserializeObject(serialisedParameter, typeof(QuadraticEquationGeneratorParameters));

                if (quadraticParameter == null)
                {
                    yield return new ValidationResult("Not given quadratic parameters.");
                }
            }
            else if (QuestionType == "simultaneous")
            {
                var serialisedParameter = JsonConvert.SerializeObject(Parameter);
                var quadraticParameter = JsonConvert.DeserializeObject(serialisedParameter, typeof(LinearSimultaneousEquationsGeneratorParameters));

                if (quadraticParameter == null)
                {
                    yield return new ValidationResult("Not given simultaneous parameters.");
                }
            }
            else
            {
                yield return new ValidationResult("That type is not yet implemented.");
            }
        }
    }
}