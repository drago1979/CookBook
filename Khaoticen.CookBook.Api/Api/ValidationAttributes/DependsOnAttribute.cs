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

