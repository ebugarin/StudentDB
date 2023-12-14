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
public class Program
{
    static async Task Main()
    {
        try
        {
            var studentCount = await ApiInteraction.GetStudentCountFromApi();
            Console.WriteLine($"Total number of students: {studentCount}");

            using (var dbContext = new SchoolDbContext())
            {
                await DatabaseInteraction.PopulateDatabaseAsync(dbContext, studentCount);
            }

            Console.WriteLine("Done.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    
}
