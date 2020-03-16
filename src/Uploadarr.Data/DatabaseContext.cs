using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using Uploadarr.Common;


namespace Uploadarr.Data
{
    public sealed class DatabaseContext : DbContext
    {
        private readonly string connectionString = "Data Source=./UploadarrDB.db";

        public DbSet<DirectoryPath> DirectoryPath { get; set; }
        public DbSet<DirectoryPathType> DirectoryPathType { get; set; }

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // import all separate entity config files
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

