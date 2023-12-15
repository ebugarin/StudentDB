using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDB.DataModels;
using StudentDB.DatabaseInteraction;
using StudentDB.ApiInteraction;

public class Program2
{
    public static async Task<List<StudentDetails>> GetStudentsWithMoreThan10CreditHoursAndFailing()
    {
        var studentsWithMoreThan10CreditHours = new List<StudentDetails>();
        var studentCount = await ApiInteraction.GetStudentCountFromApi();

        for (int studentId = 1; studentId <= studentCount; studentId++)
        {
            var studentDetails = await ApiInteraction.GetStudentDetailsFromApi(studentId);

            if (studentDetails.Id != 0)
            {
                var totalCreditHours = studentDetails.Courses
                    .Where(course => course.Status == "Enrolled")
                    .Sum(course => course.CreditHours);
                var enrolledCourses = studentDetails.Courses
                .Where(course => course.Status == "Enrolled" && course.Grade < 40)
                .ToList();

                if (totalCreditHours > 10)
                {
                    studentsWithMoreThan10CreditHours.Add(new StudentDetails
                    {
                        Id = studentDetails.Id,
                        Name = studentDetails.Name,
                        Email = studentDetails.Email,
                        Courses = enrolledCourses,
                        TotalCreditHours = totalCreditHours
                    });
                }
            }
        }

        return studentsWithMoreThan10CreditHours;
    }

    public static async Task DoProgram2()
    {
        var apiBaseUrl = "http://university.pinnstrat.com:8888";
        var httpClient = new HttpClient();

        // Fetch all students with more than 10 credit hours from the API
        var students = await GetStudentsWithMoreThan10CreditHoursAndFailing();
        int i = 0;

        // Process each student
        foreach (var student in students)
        {
            // Fetch the list of courses for the student
            var courses = student.Courses;

            // Sort courses by grade in ascending order
            var coursesToDrop = courses
                .Where(course => course.Status == "Enrolled")
                .OrderBy(course => course.Grade)
                .ToList();

            // Drop courses until the student has at least 10 credit hours
            var remainingCreditHours = student.TotalCreditHours;


            foreach (var course in coursesToDrop)
            {
                if (remainingCreditHours >= 13)
                {
                    // Drop the course
                    await DropCourseForStudent(apiBaseUrl, httpClient, course.Id, student.Id);

                    // Update remaining credit hours
                    remainingCreditHours -= course.CreditHours;

                    //Console.WriteLine($"Dropped course {course.Title} for student {student.Name}");
                    i++;
                }
                else
                {
                    break; // Stop dropping courses once 10 credit hours are maintained
                }
            }
        }

        //Console.WriteLine($"Processing completed. Dropped {i}");
    }

    static async Task DropCourseForStudent(string apiBaseUrl, HttpClient httpClient, int courseId, int studentId)
    {
        // Drop the course via API
        var dropCourseApiUrl = $"{apiBaseUrl}/Course/{courseId}/Drop/{studentId}";
        var response = await httpClient.PostAsync(dropCourseApiUrl, null);
        response.EnsureSuccessStatusCode();
    }
}


