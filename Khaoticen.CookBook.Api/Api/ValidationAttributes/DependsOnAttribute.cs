using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Khaoticen.CookBook.Api.Api.ValidationAttributes;

public class DependsOnAttribute: ValidationAttribute
{
    private readonly string _otherProperty;

    public DependsOnAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
        ErrorMessage = "{0} requires a non-empty value in {1}.";
    }

    /// <summary>
    /// Validates the current property based on the value of another property specified by name.
    /// Ensures that a non-empty value in the current property requires a non-empty value in the specified related property.
    /// </summary>
    /// <param name="value">The value of the current property to validate.</param>
    /// <param name="validationContext">Contextual information about the validation operation,
    /// including the instance and metadata of the object being validated.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating validation success or failure.
    /// Returns <see cref="ValidationResult.Success"/> if validation is successful,
    /// or a <see cref="ValidationResult"/> with an error message if validation fails.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var otherProp = validationContext.ObjectType.GetProperty(_otherProperty, BindingFlags.Public | BindingFlags.Instance);

        if (otherProp == null)
            return new ValidationResult($"Unknown property: {_otherProperty}");

        var otherValue = otherProp.GetValue(instance);

        bool thisHasValue = value is string s1 && !string.IsNullOrWhiteSpace(s1);
        bool otherHasValue = otherValue is string s2 && !string.IsNullOrWhiteSpace(s2);
        
        if (thisHasValue && !otherHasValue)
        {
            return new ValidationResult(
                string.Format(ErrorMessage!, validationContext.MemberName, _otherProperty),
                new[] { validationContext.MemberName!, _otherProperty }
            );
        }

        return ValidationResult.Success;
    }
}

