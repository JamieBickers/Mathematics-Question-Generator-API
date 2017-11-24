using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels
{
    public abstract class QuestionGenerator<QuestionType> : IQuestionGenerator<QuestionType>
    {
        protected const int MaxNumberOfTries = 1000000;
        protected IRandomIntegerGenerator randomIntegerGenerator;

        public QuestionGenerator(IRandomIntegerGenerator randomIntegerGenerator)
        {
            this.randomIntegerGenerator = randomIntegerGenerator;
        }

        public abstract QuestionType GenerateQuestionAndAnswer();
    }
}