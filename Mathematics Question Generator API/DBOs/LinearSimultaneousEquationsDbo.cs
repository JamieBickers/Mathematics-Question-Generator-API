using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using System.ComponentModel.DataAnnotations.Schema;
using MathematicsQuestionGeneratorAPI.Models;
using System;
using System.Collections.Generic;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class LinearSimultaneousEquationsDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int FirstEquationXTerm { get; set; }
        public int FirstEquationYTerm { get; set; }
        public int FirstEquationConstantTerm { get; set; }
        public int SecondEquationXTerm { get; set; }
        public int SecondEquationYTerm { get; set; }
        public int SecondEquationConstantTerm { get; set; }

        public LinearSimultaneousEquationsDbo() { }

        public LinearSimultaneousEquationsDbo(LinearSimultaneousEquations equations)
        {
            FirstEquationXTerm = equations.Coefficients[0].XTerm;
            FirstEquationYTerm = equations.Coefficients[0].YTerm;
            FirstEquationConstantTerm = equations.Coefficients[0].ConstantTerm;

            SecondEquationXTerm = equations.Coefficients[1].XTerm;
            SecondEquationYTerm = equations.Coefficients[1].YTerm;
            SecondEquationConstantTerm = equations.Coefficients[1].ConstantTerm;
        }

        public LinearSimultaneousEquations CovertToLinearSimultaneousEquations()
        {
            var equations = new List<LinearEquation>()
            {
                    new LinearEquation(FirstEquationXTerm, FirstEquationYTerm, FirstEquationConstantTerm),
                    new LinearEquation(SecondEquationXTerm, SecondEquationYTerm, SecondEquationConstantTerm)
            };

            var solution = LinearSimultaneousEquationsAnalysisFunctions.CalculateSolution(equations, out var invalidSolution);

            if (invalidSolution)
            {
                throw new Exception("Data was stored incorrectly.");
            }

            return new LinearSimultaneousEquations(equations, solution);
        }
    }
}