using API_QUIZZ.Models;
using Microsoft.EntityFrameworkCore;

namespace API_QUIZZ.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Questions> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
