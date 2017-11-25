using MathematicsQuestionGeneratorAPI.Exceptions;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MathematicsQuestionGeneratorAPI.Models.QuadraticEquations
{
    public class QuadraticEquationGeneratorParameters : IValidatableObject
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
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((ALowerBound > AUpperBound) || (BLowerBound > BUpperBound) || (CLowerBound > CUpperBound))
            {
                yield return new ValidationResult("Upper bound is greater than lower bound for one of the coefficients.");
            }
            else if ((RequireIntegerRoot || RequireRealRoot) && RequireComplexRoot)
            {
                yield return new ValidationResult("Cannot have a complex root alongside either a reel root or an integer root.");
            }
            else if (RequireDoubleRoot && RequireComplexRoot)
            {
                yield return new ValidationResult("Cannot have a double complex root.");
            }
        }
    }
}