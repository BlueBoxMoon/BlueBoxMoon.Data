using BlueBoxMoon.Data.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace Console.Runner
{
    public class DatabaseContext : EntityDbContext
    {
        public DbSet<Person> People { get; set; }

        public DatabaseContext( DbContextOptions<DatabaseContext> contextOptions )
            : base( contextOptions )
        {
        }
    }
}
