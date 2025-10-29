using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.ValidationAttributes;

public class StringGuidCollectionAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates whether the provided value is an enumerable of valid GUID strings.
    /// </summary>
    /// <param name="value">The value to validate, expected to be an enumerable of strings.</param>
    /// <param name="validationContext">The context in which the validation is performed, providing information about the object being validated.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid.
    /// Returns <c>ValidationResult.Success</c> if all items are valid GUID strings.
    /// Otherwise, returns a <see cref="ValidationResult"/> with an error message specifying the issue.
    /// </returns>
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