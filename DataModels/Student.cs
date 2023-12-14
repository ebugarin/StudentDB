namespace StudentDB.DataModels
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public double TotalCreditHours { get; set; }
        public double CurrentSemesterGPA { get; set; }
    }
}