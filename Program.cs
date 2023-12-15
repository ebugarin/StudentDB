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
            // Program 1: Build a database from the student API
            var studentCount = await ApiInteraction.GetStudentCountFromApi();
            Console.WriteLine($"Total number of students: {studentCount}");

            using (var dbContext = new SchoolDbContext())
            {
                await DatabaseInteraction.PopulateDatabaseAsync(dbContext, studentCount);
            }

            Console.WriteLine("Done.");
            var apiBaseUrl = "http://university.pinnstrat.com:8888";
            var httpClient = new HttpClient();

            
            // Program 2 below
            // Call the POST /Reset API
            await ResetApi(apiBaseUrl, httpClient);

            // Call the doProgram2 method from Program2
            await Program2.DoProgram2();

            // Call the GET /Validate API
            await ValidateApi(apiBaseUrl, httpClient);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
     static async Task ValidateApi(string apiBaseUrl, HttpClient httpClient)
        {
            var validateApiUrl = $"{apiBaseUrl}/Validate";

            // Make a GET request to the Validate API
            var response = await httpClient.GetAsync(validateApiUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("GET /Validate returned 200. Auto-Drop activity successful.");
            }
            else
            {
                Console.WriteLine($"GET /Validate returned {response.StatusCode}. Auto-Drop activity unsuccessful.");
            }
        }

        static async Task ResetApi(string apiBaseUrl, HttpClient httpClient)
        {
            var resetApiUrl = $"{apiBaseUrl}/Reset";

            // Make a POST request to the Reset API
            var response = await httpClient.PostAsync(resetApiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("POST /Reset returned 200. Database re-initialized successfully.");
            }
            else
            {
                Console.WriteLine($"POST /Reset returned {response.StatusCode}. Database re-initialization unsuccessful.");
            }
        }
    

    
}
