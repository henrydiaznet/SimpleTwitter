using System.ComponentModel.DataAnnotations;

namespace SimpleTwitter.Api.Validation;

internal static class Annotations
{
    public static IEnumerable<ValidationError> Validate(object? obj)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(obj);
        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (isValid) return [];
        
        var errors = validationResults.Select(vr => new ValidationError{ MemberNames = vr.MemberNames, ErrorMessage = vr.ErrorMessage });
        return errors;
    }
}