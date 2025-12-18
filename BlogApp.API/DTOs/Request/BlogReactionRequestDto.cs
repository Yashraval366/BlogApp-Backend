using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.API.Data.Entities;
using BlogApp.API.Enums;

namespace BlogApp.API.DTOs.Request
{
    public class BlogReactionRequestDto
    {

        [Required]
        public int BlogId { get; set; }
        [Required]
        public ReactionEnum ReactionType { get; set; }

    }
}
