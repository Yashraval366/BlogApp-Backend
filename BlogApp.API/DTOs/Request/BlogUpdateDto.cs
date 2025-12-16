using System.ComponentModel.DataAnnotations;

namespace BlogApp.API.DTOs.Request
{
    public class BlogUpdateDto
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
