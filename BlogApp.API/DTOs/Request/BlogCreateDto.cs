using System.ComponentModel.DataAnnotations;
using BlogApp.API.Enums;

namespace BlogApp.API.DTOs.Request
{
    public class BlogCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string BlogVisibility { get; set; }
    }
}
