using MathematicsQuestionGeneratorAPI.Exceptions;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGeneratorParameters : QuestionParameters
    {
        [DefaultValue(-10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ALowerBound { get; set; }

        [DefaultValue(-100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int BLowerBound { get; set; }

        [DefaultValue(-100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CLowerBound { get; set; }

        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int AUpperBound { get; set; }

        [DefaultValue(100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int BUpperBound { get; set; }

        [DefaultValue(100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CUpperBound { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireIntegerRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireRealRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireComplexRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireDoubleRoot { get; set; }

        public QuadraticEquationGeneratorParameters(int aLowerBound = -10, int bLowerBound = -100, int cLowerBound = -100, int aUpperBound = 10,
            int bUpperBound = 100, int cUpperBound = 100, bool requireIntegerRoot = false,
            bool requireRealRoot = false, bool requireComplexRoot = false, bool requireDoubleRoot = false)
        {
            ALowerBound = aLowerBound;
            BLowerBound = bLowerBound;
            CLowerBound = cLowerBound;
            AUpperBound = aUpperBound;
            BUpperBound = bUpperBound;
            CUpperBound = cUpperBound;
            RequireIntegerRoot = requireIntegerRoot;
            RequireRealRoot = requireRealRoot;
            RequireComplexRoot = requireComplexRoot;
            RequireDoubleRoot = requireDoubleRoot;

            if (!CheckValidParameters())
            {
                throw new InvalidParametersException();
            }
        }

        protected override bool CheckValidParameters()
        {
            if ((ALowerBound > AUpperBound) || (BLowerBound > BUpperBound) || (CLowerBound > CUpperBound))
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
    }
}