using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.ValidationAttributes;

public class StringGuidAttribute : ValidationAttribute
{
    /// Validates whether the provided value is a valid GUID in string or Guid format.
    /// <param name="value">The value to validate, which may be a string or Guid.</param>
    /// <param name="validationContext">Contextual information about the validation operation.</param>
    /// <return>
    /// A ValidationResult indicating success if the value is a valid GUID,
    /// otherwise containing an error message specifying the failure reason.
    /// </return>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && Guid.TryParse(str, out _))
            return ValidationResult.Success;

        if (value is Guid)
            return ValidationResult.Success;
        
        return new ValidationResult($"The {validationContext.DisplayName} field must be a valid GUID.");
    }
}