using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class WorksheetGeneratorParameters
    {
        public string EmailAddress;
        public List<QuadraticEquationGeneratorParameters> QuadraticEquationParameters;

        public WorksheetGeneratorParameters(string emailAddress, List<QuadraticEquationGeneratorParameters> quadraticEquationParameters)
        {
            EmailAddress = emailAddress;
            QuadraticEquationParameters = quadraticEquationParameters;
        }
    }
}
