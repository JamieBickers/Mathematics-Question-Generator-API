using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public interface IQuestionGenerator<out QuestionType>
    {
        QuestionType GenerateQuestionAndAnswer();
    }
}
