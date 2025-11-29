using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class UsernameAttribute : ValidationAttribute, IClientModelValidator
{
    private const string Pattern = @"^[A-Za-z][A-Za-z0-9_-]{0,19}$";

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value == null)
            return ValidationResult.Success; // [Required] handles empties

        var str = value.ToString();

        if (!Regex.IsMatch(str!, Pattern))
        {
            return new ValidationResult(ErrorMessage ??
                "Must start with a letter and contain only letters, digits, - or _. Max 20 characters.");
        }

        return ValidationResult.Success;
    }

    // ✅ Enables client-side (jQuery unobtrusive)
    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-username"] =
            ErrorMessage ?? "Invalid username format.";
        context.Attributes["data-val-username-pattern"] = Pattern;
    }
}
