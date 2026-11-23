using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Models;

public class AppUser
{
    public static List<string> Roles { get; } = new() { "Student", "Instructor", "Staff", "Admin" };

    public int AppUserID { get; set; }

    [Required(ErrorMessage = "Please enter a name.")]
    [RegularExpression("[A-Za-z ]+", ErrorMessage = "Name may contain letters and spaces only.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "Please enter a date of birth.")]
    [DataType(DataType.Date)]
    [PastBirthDate]
    public DateTime DOB { get; set; }

    [Required(ErrorMessage = "Please enter an email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(254, ErrorMessage = "Email address must be 254 characters or fewer.")]
    [Remote("CheckEmail", "Validation", "Admin", HttpMethod = "POST", AdditionalFields = nameof(AppUserID) + ",__RequestVerificationToken")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter an SSN.")]
    [RegularExpression("[0-9-]+", ErrorMessage = "SSN may contain digits and dashes only.")]
    public string SSN { get; set; } = string.Empty;

    [Display(Name = "User Type")]
    [Required(ErrorMessage = "Please select a user type.")]
    [AllowedValues("Student", "Instructor", "Staff", "Admin", ErrorMessage = "Please select a valid user type.")]
    public string Role { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
