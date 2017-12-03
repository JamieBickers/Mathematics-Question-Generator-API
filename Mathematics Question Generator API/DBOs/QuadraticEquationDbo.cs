using MathematicsQuestionGeneratorAPI.Models.QuadraticEquations;
using System.ComponentModel.DataAnnotations.Schema;
using MathematicsQuestionGeneratorAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class QuadraticEquationDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int ATerm { get; set; }

        [Required]
        public int BTerm { get; set; }

        [Required]
        public int CTerm { get; set; }

        public QuadraticEquationDbo() { }

        public QuadraticEquationDbo(QuadraticEquation equation)
        {
            ATerm = equation.Coefficients[0];
            BTerm = equation.Coefficients[1];
            CTerm = equation.Coefficients[2];
        }

        public QuadraticEquation CovertToQuadraticEquation()
        {
            return new QuadraticEquation(
                new List<int>() { ATerm, BTerm, CTerm },
                QuadraticEquationAnalysisFunctions.ComputeRoots(ATerm, BTerm, CTerm));
        }
    }
}