using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDB.DataModels;
using StudentDB.DatabaseInteraction;

namespace StudentDB.ApiInteraction
{
    public static class ApiInteraction
    {
        public static async Task<int> GetStudentCountFromApi()
        {
            string apiUrl = "http://university.pinnstrat.com:8888/Student";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonResponse);
                int arrayLength = jsonArray.Count;

                return arrayLength;
            }
        }

        public static async Task<StudentDetails> GetStudentDetailsFromApi(int studentId)
        {
            var apiBaseUrl = "http://university.pinnstrat.com:8888";
            var httpClient = new HttpClient();

            // Make a GET request to retrieve student details by ID
            var studentDetailsJson = await httpClient.GetStringAsync($"{apiBaseUrl}/Student/{studentId}");

            // Deserialize the JSON response to a StudentDetails object
            var studentDetails = JsonConvert.DeserializeObject<StudentDetails>(studentDetailsJson);

            return studentDetails;
        }
    }
}
