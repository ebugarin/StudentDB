using System.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using StudentDB.ApiInteraction;
using StudentDB.DataModels;

namespace StudentDB.DatabaseInteraction
{
    public static class DatabaseInteraction
    {
        
        public static double GradeToPoint(double grade)
        {
            if (grade >= 90) return 4.0;
            else if (grade >= 80) return 3.0;
            else if (grade >= 70) return 2.0;
            else if (grade >= 60) return 1.0;
            else return 0.0;
        }
        public static async Task PopulateDatabaseAsync(SchoolDbContext dbContext, int studentCount)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            for (int studentId = 1; studentId <= studentCount; studentId++)
            {
                var studentDetails = await ApiInteraction.ApiInteraction.GetStudentDetailsFromApi(studentId);

                if (studentDetails.Id != 0)
                {
                    var totalCreditHours = studentDetails.Courses
                        .Where(course => course.Status == "Enrolled")
                        .Sum(course => course.CreditHours);

                    var totalPoints = studentDetails.Courses
                        .Where(course => course.Status == "Enrolled")
                        .Sum(course => GradeToPoint(course.Grade) * course.CreditHours);

                    var gpa = totalCreditHours > 0 ? Math.Round(totalPoints / totalCreditHours, 2) : 0;
                    Console.WriteLine($"GPA; {gpa}");

                    dbContext.Students.Add(new Student
                    {
                        Id = studentDetails.Id,
                        Name = studentDetails.Name,
                        Email = studentDetails.Email,
                        TotalCreditHours = totalCreditHours,
                        CurrentSemesterGPA = gpa
                    });

                    await dbContext.SaveChangesAsync();
                }
            }
        }

    }
}
