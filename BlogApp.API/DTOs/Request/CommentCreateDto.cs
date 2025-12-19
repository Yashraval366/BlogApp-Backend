using System.ComponentModel.DataAnnotations;

namespace BlogApp.API.DTOs.Request
{
    public class CommentCreateDto
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
