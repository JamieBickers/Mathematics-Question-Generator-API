using System;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGeneratorParameters
    {
        public int ALowerBound { get; set; } = -10;
        public int BLowerBound { get; set; } = -100;
        public int CLowerBound { get; set; } = -100;

        public int AUpperBound { get; set; } = 10;
        public int BUpperBound { get; set; } = 100;
        public int CUpperBound { get; set; } = 100;

        public int DecimalPlaces { get; set; } = 2;

        public bool RequireIntegerRoot { get; set; } = false;
        public bool RequireRealRoot { get; set; } = false;
        public bool RequireComplexRoot { get; set; } = false;
        public bool RequireDoubleRoot { get; set; } = false;

        public QuadraticEquationGeneratorParameters(int aLowerBound = -10, int bLowerBound = -100, int cLowerBound = -100, int aUpperBound = 10,
            int bUpperBound = 100, int cUpperBound = 100, int decimalPlaces = 2, bool requireIntegerRoot = false,
            bool requireRealRoot = false, bool requireComplexRoot = false, bool requireDoubleRoot = false)
        {
            ALowerBound = aLowerBound;
            BLowerBound = bLowerBound;
            CLowerBound = cLowerBound;
            AUpperBound = aUpperBound;
            BUpperBound = bUpperBound;
            CUpperBound = cUpperBound;
            DecimalPlaces = decimalPlaces;
            RequireIntegerRoot = requireIntegerRoot;
            RequireRealRoot = requireRealRoot;
            RequireComplexRoot = requireComplexRoot;
            RequireDoubleRoot = requireDoubleRoot;
        }

        public bool CheckValidParameters()
        {
            if ((ALowerBound > AUpperBound) || (BLowerBound > BUpperBound) || (CLowerBound > CUpperBound))
            {
                return false;
            }
            else if (DecimalPlaces < 0)
            {
                return false;
            }
            else if ((RequireIntegerRoot || RequireRealRoot) && RequireComplexRoot)
            {
                return false;
            }
            else if (RequireDoubleRoot && RequireComplexRoot)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //TODO: Replace this with something less ugly.
        public void Fill()
        {
            var defaultParameters = new QuadraticEquationGeneratorParameters();
            Type type = typeof(QuadraticEquationGeneratorParameters);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                var defaultValue = property.GetValue(defaultParameters);
                if (value.GetType() == typeof(int) && (int) value == 0)
                {
                    property.SetValue(this, defaultValue);
                }
                else if (value.GetType() == typeof(bool) && (bool)value == false)
                {
                    property.SetValue(this, defaultValue);
                }
            }
        }
    }
}