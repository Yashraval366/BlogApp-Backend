using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.API.Enums;

namespace BlogApp.API.Data.Entities
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public int ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public BlogVisibility BlogVisibility { get; set; } = BlogVisibility.Public;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastUpdatedAt { get; set; }

        public ICollection<BlogReaction>? Reactions { get; set; }

    }
}
