using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators
{
    public class FixedRandomIntegerGenerator : IRandomIntegerGenerator
    {
        private const int seed = 1000;
        private Random builtInRandom;

        public FixedRandomIntegerGenerator()
        {
            builtInRandom = new Random(seed);
        }
        public int GenerateRandomInteger(int lowerBound, int upperBound)
        {
            return builtInRandom.Next(lowerBound, upperBound);
        }

        public int GenerateRandomInteger(int upperBound)
        {
            return builtInRandom.Next(upperBound);
        }
    }
}
