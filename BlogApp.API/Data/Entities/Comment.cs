using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.API.Data.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [ForeignKey("Blog")]
        public int BlogId { get; set; }

        public Blog? Blog { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [ForeignKey("Comment")]
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

        public ICollection<Comment>? Replies { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}
