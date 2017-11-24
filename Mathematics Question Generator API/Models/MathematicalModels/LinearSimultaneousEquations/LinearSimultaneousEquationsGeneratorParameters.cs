using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGeneratorParameters : QuestionParameters
    {
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireUniqueSolution { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireNoSolutions { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireInfiniteSolutions { get; set; }

        [DefaultValue(-100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CoefficientLowerBound { get; set; }

        [DefaultValue(100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CoefficientUpperBound { get; set; }

        public LinearSimultaneousEquationsGeneratorParameters(bool requireUniqueSolution = false, bool requireNoSolutions = false,
            bool requireInfiniteSolutions = false, int coefficientLowerBound = -100, int coefficientUpperBound = 100)
        {
            RequireUniqueSolution = requireUniqueSolution;
            RequireNoSolutions = requireNoSolutions;
            RequireInfiniteSolutions = requireInfiniteSolutions;
            CoefficientLowerBound = coefficientLowerBound;
            CoefficientUpperBound = coefficientUpperBound;
            if (!CheckValidParameters())
            {
                throw new Exception("Invalid parameters.");
            }
        }

        protected override bool CheckValidParameters()
        {
            if (RequireUniqueSolution && (RequireNoSolutions || RequireInfiniteSolutions))
            {
                return false;
            }
            else if (RequireNoSolutions && RequireInfiniteSolutions)
            {
                return false;
            }
            else if (CoefficientLowerBound > CoefficientUpperBound)
            {
                return false;
            }
            return true;
        }
    }
}
