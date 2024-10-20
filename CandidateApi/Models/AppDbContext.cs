using Microsoft.EntityFrameworkCore;

namespace CandidateApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; } // Collection of candidates in the database
    }
}
