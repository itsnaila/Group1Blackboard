using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group1Blackboard.Models;

public sealed class PastBirthDateAttribute : ValidationAttribute, IClientModelValidator
{
    public PastBirthDateAttribute()
        : base("Date of Birth must be a past date, no more than 100 years ago.")
    {
    }

    public override bool IsValid(object? value)
    {
        var today = DateTime.Today;
        return value is null ||
            value is DateTime date && date.Date >= today.AddYears(-100) && date.Date < today;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var today = DateTime.Today;
        context.Attributes.TryAdd("data-val", "true");
        context.Attributes.TryAdd("data-val-pastbirthdate", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        context.Attributes.TryAdd("data-val-pastbirthdate-min", ToIsoDate(today.AddYears(-100)));
        context.Attributes.TryAdd("data-val-pastbirthdate-max", ToIsoDate(today.AddDays(-1)));
    }

    private static string ToIsoDate(DateTime date) =>
        date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
