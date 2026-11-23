using System.Data.Common;
using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Areas.Admin.Controllers;

[Area("Admin")]
public class CourseController(BlackboardContext context, ILogger<CourseController> logger) : Controller
{
    public IActionResult Index()
    {
        var courses = context.Courses
            .Include(c => c.Major)
            .Include(c => c.Instructor)
            .ToList();

        return View(courses);
    }

    [HttpGet]
    public IActionResult Add()
    {
        LoadDropDowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Course course)
    {
        course.CourseID = 0;
        return Save(course, editing: false);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var course = context.Courses.Find(id);
        if (course is null)
        {
            return NotFound();
        }

        LoadDropDowns();
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Course course) => Save(course, editing: true);

    private IActionResult Save(Course course, bool editing)
    {
        var view = editing ? nameof(Edit) : nameof(Add);

        AdminFormValidation.UseRequiredMessageForBlank(ModelState, nameof(Course.Credit), "Please enter the course credit.");
        AdminFormValidation.UseRequiredMessageForBlank(ModelState, nameof(Course.MajorID), "Please select a major.");

        try
        {
            LoadDropDowns();
            // SQLite starts a write transaction before checking uniqueness for a potential save.
            // This keeps concurrent admin saves from both accepting the same new value.
            using var transaction = ModelState.IsValid ? context.Database.BeginTransaction() : null;

            if (ModelState.GetFieldValidationState(nameof(Course.CourseNumber)) != ModelValidationState.Invalid &&
                ValidationController.CourseNumberError(context, course.CourseNumber, course.CourseID) is { } error)
            {
                ModelState.AddModelError(nameof(Course.CourseNumber), error);
            }

            if (ModelState.GetFieldValidationState(nameof(Course.MajorID)) != ModelValidationState.Invalid &&
                !context.Majors.Any(major => major.MajorID == course.MajorID))
            {
                ModelState.AddModelError(nameof(Course.MajorID), "Please select a major.");
            }

            if (ModelState.GetFieldValidationState(nameof(Course.InstructorID)) != ModelValidationState.Invalid &&
                !context.AppUsers.Any(user => user.AppUserID == course.InstructorID && user.Role == "Instructor"))
            {
                ModelState.AddModelError(nameof(Course.InstructorID), "Please select an instructor.");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, AdminFormValidation.CorrectionMessage);
                return View(view, course);
            }

            if (editing)
            {
                var existing = context.Courses.Find(course.CourseID);
                if (existing is null)
                {
                    ModelState.AddModelError(string.Empty, "This course no longer exists. Return to the list and select another course.");
                    return View(view, course);
                }

                context.Entry(existing).CurrentValues.SetValues(course);
            }
            else
            {
                context.Courses.Add(course);
            }

            context.SaveChanges();
            transaction!.Commit();
        }
        catch (Exception exception) when (exception is DbException or DbUpdateException)
        {
            logger.LogError(exception, "Could not save the admin course form.");
            ModelState.AddModelError(string.Empty, "The course could not be saved. Please try again.");
            ViewBag.Majors ??= Array.Empty<SelectListItem>();
            ViewBag.Instructors ??= Array.Empty<SelectListItem>();
            return View(view, course);
        }

        TempData["message"] = editing ? "Course updated successfully." : "Course added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var course = context.Courses
            .Include(c => c.Major)
            .FirstOrDefault(c => c.CourseID == id);

        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var course = context.Courses.Find(id);

        if (course != null)
        {
            context.Courses.Remove(course);
            context.SaveChanges();

            TempData["message"] = "Course deleted successfully.";
        }

        return RedirectToAction("Index");
    }

    private void LoadDropDowns()
    {
        ViewBag.Majors = new SelectList(context.Majors.ToList(), "MajorID", "Name");
        ViewBag.Instructors = new SelectList(
            context.AppUsers.Where(user => user.Role == "Instructor").ToList(), "AppUserID", "Name");
    }
}
