namespace StudentDB.DataModels
{
    public class StudentDetails
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public double TotalCreditHours { get; set; }

        public List<CourseDetail>? Courses { get; set; }
    }
}