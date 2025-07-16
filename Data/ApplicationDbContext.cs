using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Models;

namespace Pozdravlyator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Birthday> Birthdays { get; set; }
    }
} 