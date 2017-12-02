using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class Root
    {
        public double RealPart;
        public double ImaginaryPart;
        public int Degree;
        public bool IsInteger;

        public Root(double realPart, double imaginaryPart, int degree, bool isInteger)
        {
            RealPart = realPart;
            ImaginaryPart = imaginaryPart;
            Degree = degree;
            IsInteger = isInteger;
        }
    }
}
