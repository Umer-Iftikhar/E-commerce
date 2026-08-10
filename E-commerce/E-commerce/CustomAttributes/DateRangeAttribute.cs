using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_commerce.CustomAttributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private static readonly DateOnly MinDate = new(2025, 12, 1);

        public DateRangeAttribute() 
        {
            ErrorMessage = "Date must be between 01-Dec-2025 and today.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) 
            {
                return ValidationResult.Success;
            }

            if (value is not DateOnly date)
            {
                return new ValidationResult("Invalid date.");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            if (date < MinDate || date > today)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
