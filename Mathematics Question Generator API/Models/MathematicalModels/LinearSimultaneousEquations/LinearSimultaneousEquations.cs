using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.LinearSimultaneousEquations
{
    public class LinearSimultaneousEquations
    {
        public LinearEquation FirstEquation;
        public LinearEquation SecondEquation;
        public LinearSimultaneousEquationsSolution Solution;

        public LinearSimultaneousEquations(LinearEquation firstEquation, LinearEquation secondEquation, LinearSimultaneousEquationsSolution solution)
        {
            FirstEquation = firstEquation;
            SecondEquation = secondEquation;
            Solution = solution;
        }
    }
}
