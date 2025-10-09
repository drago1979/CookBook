using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Validation;

public class StringGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && Guid.TryParse(str, out _))
            return ValidationResult.Success;

        if (value is Guid)
            return ValidationResult.Success;
        
        return new ValidationResult($"The {validationContext.DisplayName} field must be a valid GUID.");
    }
}