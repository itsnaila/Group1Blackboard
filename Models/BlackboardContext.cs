
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Models
{
    public class BlackboardContext : DbContext
    {
        public BlackboardContext(
            DbContextOptions<BlackboardContext> options)
            : base(options)
        {
        }

        public DbSet<Major> Majors { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
        
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Admin forms require an instructor, but existing courses can be unassigned.
    modelBuilder.Entity<Course>()
        .HasOne(course => course.Instructor)
        .WithMany(instructor => instructor.Courses)
        .HasForeignKey(course => course.InstructorID)
        .IsRequired(false);

    modelBuilder.Entity<Major>().HasData(
        new Major
        {
            MajorID = 101,
            Name = "Business Administration",
            College = "College of Business"
        },
        new Major
        {
            MajorID = 102,
            Name = "Computer Science",
            College = "College of Arts and Sciences"
        },
        new Major
        {
            MajorID = 103,
            Name = "Elementary Education",
            College = "College of Education"
        },
        new Major
        {
            MajorID = 104,
            Name = "Health Administration",
            College = "College of Health and Human Services"
        },
        new Major
        {
            MajorID = 105,
            Name = "Information Systems",
            College = "College of Business"
        },
        new Major
        {
            MajorID = 106,
            Name = "Mathematics",
            College = "College of Arts and Sciences"
        }
    );
    
modelBuilder.Entity<AppUser>().HasData(
    new AppUser
    {
        AppUserID = 201,
        Name = "John Smith",
        DOB = new DateTime(1980, 5, 12),
        Email = "john.smith@example.com",
        SSN = "000-00-0001",
        Role = "Instructor"
    },
    new AppUser
    {
        AppUserID = 202,
        Name = "Mary Johnson",
        DOB = new DateTime(1985, 8, 20),
        Email = "mary.johnson@example.com",
        SSN = "000-00-0002",
        Role = "Instructor"
    },
    new AppUser
    {
        AppUserID = 203,
        Name = "David Wilson",
        DOB = new DateTime(1978, 3, 15),
        Email = "david.wilson@example.com",
        SSN = "000-00-0003",
        Role = "Instructor"
    },
    new AppUser
    {
        AppUserID = 204,
        Name = "Emily Davis",
        DOB = new DateTime(1988, 11, 7),
        Email = "emily.davis@example.com",
        SSN = "000-00-0004",
        Role = "Instructor"
    }
) ;
    
modelBuilder.Entity<Course>().HasData(
    new Course
    {
        CourseID = 301,
        CourseNumber = "BADM5000",
        CourseName = "Business Foundations",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "In-person",
        Times = "Monday 6:00 PM",
        ShortDescription = "Introduction to business management.",
        MajorID = 101,
        InstructorID = 201
    },
    new Course
    {
        CourseID = 302,
        CourseNumber = "BADM5400",
        CourseName = "Organizational Behavior",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "Online",
        Times = "Online",
        ShortDescription = "Study of behavior in organizations.",
        MajorID = 101,
        InstructorID = 202
    },
    new Course
    {
        CourseID = 303,
        CourseNumber = "BADM6000",
        CourseName = "Strategic Management",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Hybrid",
        Times = "Wednesday 6:00 PM",
        ShortDescription = "Business strategy and planning.",
        MajorID = 101,
        InstructorID = 203
    },
    new Course
    {
        CourseID = 304,
        CourseNumber = "CSPC6100",
        CourseName = "Programming Concepts",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "In-person",
        Times = "Tuesday 5:00 PM",
        ShortDescription = "Introduction to programming concepts.",
        MajorID = 102,
        InstructorID = 201
    },
    new Course
    {
        CourseID = 305,
        CourseNumber = "CSPC7300",
        CourseName = "Database Systems",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Online",
        Times = "Online",
        ShortDescription = "Database design and management.",
        MajorID = 102,
        InstructorID = 202
    },
    new Course
    {
        CourseID = 306,
        CourseNumber = "CSPC7550",
        CourseName = "Internet Programming",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Hybrid",
        Times = "Thursday 6:00 PM",
        ShortDescription = "Development of web applications.",
        MajorID = 102,
        InstructorID = 203
    },
    new Course
    {
        CourseID = 307,
        CourseNumber = "CSPC8720",
        CourseName = "Information Security",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "Online",
        Times = "Online",
        ShortDescription = "Principles of information security.",
        MajorID = 102,
        InstructorID = 204
    },
    new Course
    {
        CourseID = 308,
        CourseNumber = "EDUC5100",
        CourseName = "Foundations of Education",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "In-person",
        Times = "Monday 4:00 PM",
        ShortDescription = "Introduction to educational practice.",
        MajorID = 103,
        InstructorID = 201
    },
    new Course
    {
        CourseID = 309,
        CourseNumber = "HLTH5200",
        CourseName = "Healthcare Administration",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "Hybrid",
        Times = "Tuesday 6:00 PM",
        ShortDescription = "Healthcare organization and management.",
        MajorID = 104,
        InstructorID = 202
    },
    new Course
    {
        CourseID = 310,
        CourseNumber = "INFS6200",
        CourseName = "Information Systems",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Online",
        Times = "Online",
        ShortDescription = "Fundamentals of information systems.",
        MajorID = 105,
        InstructorID = 203
    },
    new Course
    {
        CourseID = 311,
        CourseNumber = "INFS6400",
        CourseName = "Systems Analysis",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "In-person",
        Times = "Wednesday 5:00 PM",
        ShortDescription = "Analysis and design of information systems.",
        MajorID = 105,
        InstructorID = 204
    },
    new Course
    {
        CourseID = 312,
        CourseNumber = "INFS7100",
        CourseName = "Data Management",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Hybrid",
        Times = "Thursday 5:00 PM",
        ShortDescription = "Managing organizational data.",
        MajorID = 105,
        InstructorID = 201
    },
    new Course
    {
        CourseID = 313,
        CourseNumber = "MATH5100",
        CourseName = "Applied Mathematics",
        Credit = 3,
        OfferedSemester = "Spring",
        Location = "In-person",
        Times = "Monday 5:00 PM",
        ShortDescription = "Mathematical methods and applications.",
        MajorID = 106,
        InstructorID = 202
    },
    new Course
    {
        CourseID = 314,
        CourseNumber = "MATH5200",
        CourseName = "Statistics",
        Credit = 3,
        OfferedSemester = "Fall",
        Location = "Online",
        Times = "Online",
        ShortDescription = "Statistical methods and analysis.",
        MajorID = 106,
        InstructorID = 203
    }
);

}

    }
    
}
