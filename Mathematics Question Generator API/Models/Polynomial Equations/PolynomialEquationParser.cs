using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathematics_Questions_Generator.Model
{
    class PolynomialEquationParser
    {
        List<int> Coefficients;
        List<double> Roots;
        int DecimalPlaces;

        public PolynomialEquationParser(List<int> coefficients, List<double> Roots, int decimalPlaces)
        {
            Coefficients = coefficients;
            this.Roots = Roots;
            DecimalPlaces = decimalPlaces;
        }

        public string ParseToString()
        {
            if (Coefficients.Count() < 2)
            {
                throw new Exception("Need at least two coefficients.");
            }
            return $"Equation: {FormattedEquation()}\nRoot{(Roots.Count() == 1 ? "" : "s")}: {FormattedRoots()}";
        }

        private string FormattedRoots()
        {
            return Roots.Select(root => Math.Round(root, DecimalPlaces).ToString()).Aggregate("", (root, next) => $"{root}, {next}");
        }

        private string FormattedEquation()
        {
            var formattedEquation = "";
            var degree = Coefficients.Count() - 1;

            formattedEquation += $"{FormatLeadingcoefficient(Coefficients.Last())}x{(degree == 1 ? "" : $"^{degree}")}";

            List<string> formattedNonLeadingNonTerminalTerms =
                Coefficients.GetRange(1, Coefficients.Count() - 2).Select(FormatNonLeadingNonTerminalcoefficient).ToList();

            for (int i = formattedNonLeadingNonTerminalTerms.Count() - 1; i >= 0; i--)
            {
                formattedEquation += (formattedNonLeadingNonTerminalTerms[i] == "+0") ? "" :
                    $"{formattedNonLeadingNonTerminalTerms[i]}x{(i + 1 == 1 ? "" : $"^{(i + 1)}")}";
            }

            formattedEquation += $"{FormattedTerminalcoefficient(Coefficients.First())}=0";

            return formattedEquation;
        }

        private string FormattedTerminalcoefficient(int coefficient)
        {
            return coefficient > 0 ? $"+{coefficient}" : coefficient.ToString();
        }

        private string FormatNonLeadingNonTerminalcoefficient(int coefficient)
        {
            if (coefficient == 1)
            {
                return "+";
            }
            else if (coefficient == -1)
            {
                return "-";
            }
            else if (coefficient >= 0)
            {
                return $"+{coefficient.ToString()}";
            }
            else
            {
                return $"{coefficient.ToString()}";
            }
        }

        private string FormatLeadingcoefficient(int leadingcoefficient)
        {
            if (leadingcoefficient == 0)
            {
                throw new Exception("Invalid polynomial, cannot have leading term be zero.");
            }
            else if (leadingcoefficient == 1)
            {
                return "";
            }
            else if (leadingcoefficient == -1)
            {
                return "-";
            }
            else
            {
                return leadingcoefficient.ToString();
            }
        }
    }
}
