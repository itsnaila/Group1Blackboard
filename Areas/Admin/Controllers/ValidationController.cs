using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class ValidationController(BlackboardContext context, ILogger<ValidationController> logger) : Controller
{
    [HttpPost]
    public IActionResult CheckEmail(string? email, int appUserID) =>
        Check(() => EmailError(context, email, appUserID));

    [HttpPost]
    public IActionResult CheckCourseNumber(string? courseNumber, int courseID) =>
        Check(() => CourseNumberError(context, courseNumber, courseID));

    internal static string? EmailError(BlackboardContext context, string? email, int id)
    {
        return !string.IsNullOrWhiteSpace(email) && context.AppUsers.Any(user =>
            user.AppUserID != id && EF.Functions.Collate(user.Email, "NOCASE") == email)
            ? $"Email address {email} is already used by another user."
            : null;
    }

    internal static string? CourseNumberError(BlackboardContext context, string? number, int id)
    {
        return !string.IsNullOrWhiteSpace(number) && context.Courses.Any(course =>
            course.CourseID != id && EF.Functions.Collate(course.CourseNumber, "NOCASE") == number)
            ? $"Course number {number} already exists."
            : null;
    }

    private IActionResult Check(Func<string?> findError)
    {
        try
        {
            var error = findError();
            return error is null ? Json(true) : Json(System.Text.Encodings.Web.HtmlEncoder.Default.Encode(error));
        }
        catch (System.Data.Common.DbException exception)
        {
            logger.LogError(exception, "Admin remote validation failed.");
            return Json("Validation is temporarily unavailable. Reload the form and try again.");
        }
    }
}
