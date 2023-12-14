namespace StudentDB.DataModels
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double CreditHours { get; set; }
        public string? Status { get; set; }
        public double Grade { get; set; }
    }
}