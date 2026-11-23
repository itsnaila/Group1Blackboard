using System.Data.Common;
using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController(BlackboardContext context, ILogger<UserController> logger) : Controller
{
    public IActionResult Index()
    {
        var users = context.AppUsers.ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(AppUser user)
    {
        user.AppUserID = 0;
        return Save(user, editing: false);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = context.AppUsers.Find(id);
        if (user is null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(AppUser user) => Save(user, editing: true);

    private IActionResult Save(AppUser user, bool editing)
    {
        var view = editing ? nameof(Edit) : nameof(Add);

        AdminFormValidation.UseRequiredMessageForBlank(ModelState, nameof(AppUser.DOB), "Please enter a date of birth.");

        try
        {
            // SQLite starts a write transaction before checking uniqueness for a potential save.
            // This keeps concurrent admin saves from both accepting the same new value.
            using var transaction = ModelState.IsValid ? context.Database.BeginTransaction() : null;

            if (ModelState.GetFieldValidationState(nameof(AppUser.Email)) != ModelValidationState.Invalid &&
                ValidationController.EmailError(context, user.Email, user.AppUserID) is { } error)
            {
                ModelState.AddModelError(nameof(AppUser.Email), error);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, AdminFormValidation.CorrectionMessage);
                return View(view, user);
            }

            if (editing)
            {
                var existing = context.AppUsers.Find(user.AppUserID);
                if (existing is null)
                {
                    ModelState.AddModelError(string.Empty, "This user no longer exists. Return to the list and select another user.");
                    return View(view, user);
                }

                context.Entry(existing).CurrentValues.SetValues(user);
            }
            else
            {
                context.AppUsers.Add(user);
            }

            context.SaveChanges();
            transaction!.Commit();
        }
        catch (Exception exception) when (exception is DbException or DbUpdateException)
        {
            logger.LogError(exception, "Could not save the admin user form.");
            ModelState.AddModelError(string.Empty, "The user could not be saved. Please try again.");
            return View(view, user);
        }

        TempData["message"] = editing ? "User updated successfully." : "User added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var user = context.AppUsers.Find(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = context.AppUsers.Find(id);

        if (user != null)
        {
            context.AppUsers.Remove(user);
            context.SaveChanges();

            TempData["message"] = "User deleted successfully.";
        }

        return RedirectToAction("Index");
    }
}
