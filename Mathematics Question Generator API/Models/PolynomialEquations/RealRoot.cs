using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class RealRoot
    {
        public double Value;
        public double Degree;
        public bool IsInteger;

        public RealRoot(double value, int degree, bool isInteger)
        {
            Value = value;
            Degree = degree;
            IsInteger = isInteger;
        }
    }
}
