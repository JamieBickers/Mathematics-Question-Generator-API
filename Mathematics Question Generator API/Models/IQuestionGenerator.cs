using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics_Questions_Generator.Model
{
    interface IQuestionGenerator<QuestionType>
    {
        QuestionType GenerateQuestionAndAnswer();
    }
}
