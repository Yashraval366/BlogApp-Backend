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

        public DbSet<BlogReaction> BlogsReactions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ApplicationUser> Users { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warning => warning.Log(RelationalEventId.PendingModelChangesWarning));
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

            builder.Entity<BlogReaction>()
                .HasOne(e => e.User)
                .WithMany(q => q.Reactions)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlogReaction>()
                .HasOne(e => e.Blog)
                .WithMany(q => q.Reactions)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlogReaction>()
              .HasIndex(x => new { x.BlogId, x.UserId })
              .IsUnique();

            builder.Entity<Comment>()
                .HasOne(e => e.User)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(e => e.Blog)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(e => e.ParentComment)
                .WithMany(q => q.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
