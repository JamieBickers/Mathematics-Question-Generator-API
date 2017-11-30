using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class QuadraticEquationDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ATerm { get; set; }
        public int BTerm { get; set; }
        public int CTerm { get; set; }

        public QuadraticEquationDbo(QuadraticEquation equation)
        {
            ATerm = equation.Coefficients[0];
            BTerm = equation.Coefficients[1];
            CTerm = equation.Coefficients[2];
        }
    }
}
