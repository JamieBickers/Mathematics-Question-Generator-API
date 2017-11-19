namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGeneratorParameters
    {
        public int ALowerBound;
        public int BLowerBound;
        public int CLowerBound;

        public int AUpperBound;
        public int BUpperBound;
        public int CUpperBound;

        public int DecimalPlaces;

        public bool RequireIntegerRoot;
        public bool RequireRealRoot;
        public bool RequireComplexRoot;
        public bool RequireDoubleRoot;

        public QuadraticEquationGeneratorParameters(int aLowerBound = -100, int bLowerBound = -100, int cLowerBound = -100, int aUpperBound = 100,
            int bUpperBound = 100, int cUpperBound = 100, int decimalPlaces = 2, bool requireIntegerRoot = false,
            bool requireRealRoot = false, bool requireComplexRoot = false, bool requireDoubleRoot = false)
        {
            this.ALowerBound = aLowerBound;
            this.BLowerBound = bLowerBound;
            this.CLowerBound = cLowerBound;
            this.AUpperBound = aUpperBound;
            this.BUpperBound = bUpperBound;
            this.CUpperBound = cUpperBound;
            this.DecimalPlaces = decimalPlaces;
            this.RequireIntegerRoot = requireIntegerRoot;
            this.RequireRealRoot = requireRealRoot;
            this.RequireComplexRoot = requireComplexRoot;
            this.RequireDoubleRoot = requireDoubleRoot;
        }
    }
}