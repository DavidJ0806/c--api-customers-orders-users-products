using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TodoApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    sealed public class PriceAttribute : ValidationAttribute
    {
        /// <summary>
        /// Overrides the isValid method to create a custom annotation for validation. regex doesn't work on decimals.
        /// </summary>
        /// <param name="value">Property to be evaluated</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {

            Regex rgx = new Regex("\\.[0-9]{2}$");
            if (!rgx.IsMatch(value.ToString()))
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}