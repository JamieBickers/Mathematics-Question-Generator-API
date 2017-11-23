using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators
{
    public class RandomIntegerGenerator : IRandomIntegerGenerator
    {
        private Random builtInRandom;

        public RandomIntegerGenerator()
        {
            builtInRandom = new Random();
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
