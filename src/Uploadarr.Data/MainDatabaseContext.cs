using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Uploadarr.Data
{
    public sealed class MainDatabaseContext : DbContext
    {
        private readonly string connectionString = "Data Source=./UploadarrDB.db";

        public DbSet<DirectoryPath> DirectoryPath { get; set; }
        public DbSet<DirectoryPathType> DirectoryPathType { get; set; }
        public DbSet<RootFolder> RootFolders { get; set; }
        public DbSet<RootFolderType> RootFolderTypes { get; set; }
        public DbSet<UnmappedFolder> UnmappedFolders { get; set; }

        public MainDatabaseContext()
        {
            Database.EnsureCreated();
        }

        public MainDatabaseContext(DbContextOptions<MainDatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RootFolderType>().HasData(
                new RootFolderType
                {
                    Id = 1,
                    Name = "Movies"
                },
                new RootFolderType
                {
                    Id = 2,
                    Name = "Series"
                }
            );

            // import all separate entity config files
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
