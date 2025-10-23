using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.ValidationAttributes;

public class StringGuidCollectionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable<string> guids)
        {
            return new ValidationResult($"The {validationContext.DisplayName} field must be a list of GUID strings.");
        }

        var invalidGuids = guids
            .Where(id => !Guid.TryParse(id, out _))
            .ToList();

        if (invalidGuids.Count > 0)
        {
            var msg = $"The {validationContext.DisplayName} field contains invalid GUID(s): {string.Join(", ", invalidGuids)}";
            return new ValidationResult(msg);
        }

        return ValidationResult.Success;
    }
}