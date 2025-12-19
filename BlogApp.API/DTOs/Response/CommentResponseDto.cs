namespace BlogApp.API.DTOs.Response
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

    }
}
