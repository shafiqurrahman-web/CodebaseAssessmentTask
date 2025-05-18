using CodebaseAssessmentTask.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CodebaseAssessmentTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<OtpVerification> OtpVerifications { get; set; }
        // Add other DbSets as needed
    }

}
