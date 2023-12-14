using Microsoft.EntityFrameworkCore;
using StudentDB.DataModels;
using StudentDB.ApiInteraction;

namespace StudentDB.DatabaseInteraction
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=myDB.db");
        }
    }
}