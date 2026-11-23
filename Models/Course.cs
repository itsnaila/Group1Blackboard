using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Models;

public class Course
{
    public static List<string> Semesters { get; } = new() { "Spring", "Fall", "Summer" };
    public static List<string> Locations { get; } = new() { "In-person", "Online", "Hybrid" };

    public int CourseID { get; set; }

    [Display(Name = "Course Number")]
    [Required(ErrorMessage = "Please enter a course number.")]
    [RegularExpression("[A-Za-z0-9]+", ErrorMessage = "Course number may contain letters and digits only.")]
    [Remote("CheckCourseNumber", "Validation", "Admin", HttpMethod = "POST", AdditionalFields = nameof(CourseID) + ",__RequestVerificationToken")]
    public string CourseNumber { get; set; } = string.Empty;

    [Display(Name = "Course Name")]
    [Required(ErrorMessage = "Please enter a course name.")]
    [RegularExpression("[A-Za-z0-9 ]+", ErrorMessage = "Course name may contain letters, digits, and spaces only.")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the course credit.")]
    [Range(1, int.MaxValue, ErrorMessage = "Credit must be a positive whole number.")]
    public int Credit { get; set; }

    [Display(Name = "Offered Semester")]
    [Required(ErrorMessage = "Please select a semester.")]
    [AllowedValues("Spring", "Fall", "Summer", ErrorMessage = "Please select a valid semester.")]
    public string OfferedSemester { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a location.")]
    [AllowedValues("In-person", "Online", "Hybrid", ErrorMessage = "Please select a valid location.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the meeting times.")]
    public string Times { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Please enter a description.")]
    public string ShortDescription { get; set; } = string.Empty;

    [Display(Name = "Major")]
    [Required(ErrorMessage = "Please select a major.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a major.")]
    public int MajorID { get; set; }

    public Major? Major { get; set; }

    [Display(Name = "Instructor")]
    [Required(ErrorMessage = "Please select an instructor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select an instructor.")]
    public int? InstructorID { get; set; }

    public AppUser? Instructor { get; set; }
}
