using System.Security.Claims;
using BlogApp.API.Data.DBcontext;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> AddComment(CommentCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var comment = new Comment
            {
                BlogId = dto.BlogId,
                UserId = userId,
                Content = dto.Content,
                ParentCommentId = dto.ParentCommentId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var response = new CommentResponseDto
            {
                Id = comment.Id,
                BlogId = comment.BlogId,
                UserId = userId,
                UserName = User.FindFirst(ClaimTypes.GivenName)!.Value,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ParentCommentId = comment.ParentCommentId
            };

            return Ok(Response<CommentResponseDto>.Ok(response, "Comment added"));
        }


        [HttpGet("{blogId}/comments")]
        public async Task<IActionResult> GetComments(int blogId)
        {
            var comments = await _context.Comments
                .Where(c => c.BlogId == blogId && !c.IsDeleted)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    BlogId = c.BlogId,
                    UserId = c.UserId,
                    UserName = c.User.FullName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    ParentCommentId = c.ParentCommentId
                })
                .ToListAsync();

            return Ok(Response<List<CommentResponseDto>>.Ok(comments,"comments fetched"));
        }
    }
}
