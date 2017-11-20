using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators
{
    public interface IRandomIntegerGenerator
    {
        int GenerateRandomInteger(int lowerBound, int upperBound);
        int GenerateRandomInteger(int upperBound);
    }
}
