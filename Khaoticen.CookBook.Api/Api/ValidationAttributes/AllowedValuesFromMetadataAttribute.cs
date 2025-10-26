using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Khaoticen.CookBook.Api.Api.ValidationAttributes;

public class AllowedValuesFromMetadataAttribute: ValidationAttribute
{
    private readonly Type _metadataType;
    private readonly string _fieldOrPropName;

    public AllowedValuesFromMetadataAttribute(Type metadataType, string fieldOrPropName)
    {
        _metadataType = metadataType;
        _fieldOrPropName = fieldOrPropName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var member = _metadataType.GetField(_fieldOrPropName)
                     ?? (MemberInfo?)_metadataType.GetProperty(_fieldOrPropName);

        if (member == null) 
            return new ValidationResult($"Metadata '{_fieldOrPropName}' not found on {_metadataType.Name}");

        var dict = member switch
        {
            FieldInfo fi => fi.GetValue(null),
            PropertyInfo pi => pi.GetValue(null),
            _ => null
        } as IDictionary;

        if (dict == null)
            return new ValidationResult($"'{_fieldOrPropName}' is not a dictionary.");

        if (value == null) return ValidationResult.Success;

        if (dict.Contains(value.ToString()!))
            return ValidationResult.Success;

        return new ValidationResult(
            $"{validationContext.MemberName}='{value}' is not allowed. Allowed: {string.Join(", ", dict.Keys.Cast<string>())}"
        );
    }
}