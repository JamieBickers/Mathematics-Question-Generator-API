using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class WorksheetGeneratorParameters<QuestionType, QuestionGeneratorParameterType>
        where QuestionType : IQuestion
        where QuestionGeneratorParameterType : QuestionParameters
    {
        public string EmailAddress;
        public List<QuestionGeneratorParameterType> QuestionGeneratorParameters;

        public WorksheetGeneratorParameters(string emailAddress, List<QuestionGeneratorParameterType> questionGeneratorParameters)
        {
            EmailAddress = emailAddress;
            QuestionGeneratorParameters = questionGeneratorParameters;
        }
    }
}
