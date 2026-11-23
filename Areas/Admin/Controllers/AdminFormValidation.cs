using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Group1Blackboard.Areas.Admin.Controllers;

internal static class AdminFormValidation
{
    internal const string CorrectionMessage = "Please correct the errors below and save again.";

    internal static void UseRequiredMessageForBlank(ModelStateDictionary modelState, string field, string message)
    {
        if (!modelState.TryGetValue(field, out var entry) || string.IsNullOrWhiteSpace(entry.AttemptedValue))
        {
            entry?.Errors.Clear();
            modelState.AddModelError(field, message);
        }
    }
}
