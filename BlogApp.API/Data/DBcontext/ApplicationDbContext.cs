using BlogApp.API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.API.Data.DBcontext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<ApplicationUser> Users { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warning =>
            {
                warning.Log(RelationalEventId.PendingModelChangesWarning);
            });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasData(

                    new Category
                    {
                        Id = 1,
                        Name = "Software Development"
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Music"
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Gaming"
                    },
                    new Category
                    {
                        Id = 4,
                        Name = "Films"
                    }
                );
        }
    }
}
