using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PomodoroScheduler.ValidationRules
{
    public class NumericRangeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (int.TryParse(value as string, out int number))
            {
                if (number < Min || number > Max)
                    return new ValidationResult(false, $"Value must be between {Min} and {Max}.");
            }
            else
            {
                return new ValidationResult(false, "Invalid number.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
