using CommonObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskObject> TaskTable { get; set; }
    }
}