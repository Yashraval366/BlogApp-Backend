using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.API.Enums;

namespace BlogApp.API.Data.Entities
{
    public class BlogReaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        [Required]
        public int UserId { get; set; }

        public ApplicationUser? User { get; set; }

        [ForeignKey(nameof(Blog))]
        [Required]
        public int BlogId { get; set; }

        public Blog? Blog { get; set; }

        public ReactionEnum ReactionType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
