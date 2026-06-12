using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CaliforniumCore.Web.Validation;

/// <summary>
/// Validates that a string is either a well-formed absolute URL or a site-relative path starting with '/'.
/// Accepts null or empty values.
/// </summary>
public class RelativeOrAbsoluteUrlAttribute : ValidationAttribute, IClientModelValidator
{
    public RelativeOrAbsoluteUrlAttribute()
    {
        ErrorMessage = "The field must be an absolute URL or a site-relative path starting with '/'.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var s = value as string;
        if (string.IsNullOrWhiteSpace(s))
        {
            return ValidationResult.Success;
        }

        s = s.Trim();

        // Allow relative site paths starting with a single '/'.
        if (s.StartsWith('/') && !s.StartsWith("//"))
        {
            return ValidationResult.Success;
        }

        // Allow well-formed absolute HTTP(S)/FTP URLs.
        if (Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp ||
             uri.Scheme == Uri.UriSchemeHttps ||
             uri.Scheme == Uri.UriSchemeFtp))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-relativeorabsoluteurl", ErrorMessage ?? "The field must be an absolute URL or a site-relative path starting with '/'.");
    }

    private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key)) return;
        attributes.Add(key, value);
    }
}
