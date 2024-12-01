using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PomodoroScheduler.ValidationRules
{
    public class MaxLengthValidationRule : ValidationRule
    {
        public int MaxLength { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is string text && text.Length > MaxLength)
            {
                return new ValidationResult(false, $"Maximum {MaxLength} characters allowed.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
