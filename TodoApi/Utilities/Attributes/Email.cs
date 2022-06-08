using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TodoApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    sealed public class EmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// Overrides the isValid method to create a custom annotation for validation. In case I want to reference this later
        /// when I need it
        /// </summary>
        /// <param name="value">Property to be evaluated</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                Regex rgx = new Regex("^[a-zA-Z0-9_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z]+\\.[a-zA-Z]+$");
                if (!rgx.IsMatch(value?.ToString()))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}