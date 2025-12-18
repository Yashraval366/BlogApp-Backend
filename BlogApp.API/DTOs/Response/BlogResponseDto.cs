using BlogApp.API.Enums;

namespace BlogApp.API.DTOs.Response
{
    public class BlogResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public BlogVisibility BlogVisibility { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public int LikeCounts { get; set; }

        public int DislikeCounts { get; set; }
        public int? UserReaction {  get; set; }
    }
}
