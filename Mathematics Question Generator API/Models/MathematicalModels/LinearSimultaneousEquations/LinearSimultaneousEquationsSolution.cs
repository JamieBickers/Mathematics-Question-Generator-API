using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsSolution
    {
        public double FirstSolution;
        public double SecondSolution;
        public bool NoSolution;
        public bool InfiniteSolutions;

        public LinearSimultaneousEquationsSolution(double firstSolution, double secondSolution, bool noSolution, bool infiniteSolutions)
        {
            FirstSolution = firstSolution;
            SecondSolution = secondSolution;
            NoSolution = noSolution;
            InfiniteSolutions = infiniteSolutions;
        }
    }
}
