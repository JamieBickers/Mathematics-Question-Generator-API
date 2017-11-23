using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public interface IQuestionGenerator<QuestionType>
    {
        QuestionType GenerateQuestionAndAnswer();
    }
}
