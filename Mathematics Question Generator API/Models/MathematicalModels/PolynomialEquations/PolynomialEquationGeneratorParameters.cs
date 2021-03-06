﻿using MathematicsQuestionGeneratorAPI.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MathematicsQuestionGeneratorAPI.Models.PolynomialEquations
{
    public class PolynomialEquationGeneratorParameters : IValidatableObject
    {
        [DefaultValue(2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Degree { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinimumNumberOfTerms { get; set; }

        [DefaultValue(-10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int LeadingTermLowerBound { get; set; }

        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int LeadingTermUpperBound { get; set; }

        [DefaultValue(-100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int OtherTermsLowerBound { get; set; }

        [DefaultValue(100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int OtherTermsUpperBound { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireAnIntegerRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireARealRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireAComplexRoot { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireOnlyComplexRoots { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RequireADoubleRoot { get; set; }

        public PolynomialEquationGeneratorParameters(int degree = 2, int leadingTermLowerBound = -10, int leadingTermUpperBound = 10, int otherTermsLowerBound = -100,
            int otherTermsUpperBound = 100, int minimumNumberOfTerms = -1,  bool requireAnIntegerRoot = false, bool requireARealRoot = false,
            bool requireAComplexRoot = false, bool requireOnlyComplexRoots = false, bool requireADoubleRoot = false)
        {
            Degree = degree;
            LeadingTermLowerBound = leadingTermLowerBound;
            LeadingTermUpperBound = leadingTermUpperBound;
            OtherTermsLowerBound = otherTermsLowerBound;
            OtherTermsUpperBound = otherTermsUpperBound;
            MinimumNumberOfTerms = minimumNumberOfTerms == -1 ? degree / 2 : minimumNumberOfTerms;
            RequireAnIntegerRoot = requireAnIntegerRoot;
            RequireARealRoot = requireARealRoot;
            RequireAComplexRoot = requireAComplexRoot;
            RequireOnlyComplexRoots = requireOnlyComplexRoots;
            RequireADoubleRoot = requireADoubleRoot;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((LeadingTermLowerBound > LeadingTermUpperBound) || (OtherTermsLowerBound > OtherTermsUpperBound))
            {
                yield return new ValidationResult("Lower bounds cannot be above upper bounds.");
            }
            if (Degree <= 0)
            {
                yield return new ValidationResult("Degree must be positive.");
            }
            if (RequireOnlyComplexRoots && RequireARealRoot)
            {
                yield return new ValidationResult("Cannot have only complex roots and a real root.");
            }
            if (RequireADoubleRoot && RequireOnlyComplexRoots)
            {
                yield return new ValidationResult("Cannot have only complex roots and a double root.");
            }
        }
    }
}
