using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class WorksheetGeneratorParameters<QuestionType, QuestionGeneratorParameterType> : IValidatableObject
        where QuestionType : IQuestion
        where QuestionGeneratorParameterType : IValidatableObject
    {
        public string EmailAddress;
        public List<QuestionGeneratorParameterType> QuestionGeneratorParameters;

        public WorksheetGeneratorParameters(string emailAddress, List<QuestionGeneratorParameterType> questionGeneratorParameters)
        {
            EmailAddress = emailAddress;
            QuestionGeneratorParameters = questionGeneratorParameters;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return QuestionGeneratorParameters.SelectMany(parameter => parameter.Validate(new ValidationContext(parameter)));
        }
    }
}
