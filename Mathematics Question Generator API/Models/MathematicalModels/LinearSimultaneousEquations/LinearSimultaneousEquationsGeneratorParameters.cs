using MathematicsQuestionGeneratorAPI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations
{
    public class LinearSimultaneousEquationsGeneratorParameters : IValidatableObject
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
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RequireUniqueSolution && (RequireNoSolutions || RequireInfiniteSolutions))
            {
                yield return new ValidationResult("Cannot have unique solution alongside either no solution or infinite solutions.");
            }
            else if (RequireNoSolutions && RequireInfiniteSolutions)
            {
                yield return new ValidationResult("Cannot have no solutions and infinite solutions.");
            }
            else if (CoefficientLowerBound > CoefficientUpperBound)
            {
                yield return new ValidationResult("Lower bound cannot be above upper bound.");
            }
        }
    }
}
