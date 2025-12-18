using System.Security.Claims;
using BlogApp.API.Data.DBcontext;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using BlogApp.API.Enums;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public BlogReactionController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost("react")]
        public async Task<IActionResult> React(BlogReactionRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var reaction = await _db.BlogsReactions
                .FirstOrDefaultAsync(e => e.UserId == userId && e.BlogId == dto.BlogId);

            var incomingReaction = (ReactionEnum)dto.ReactionType;

            if (reaction == null)
            {
                reaction = new BlogReaction
                {
                    BlogId = dto.BlogId,
                    UserId = userId,
                    ReactionType = incomingReaction
                };

                await _db.BlogsReactions.AddAsync(reaction);
            }
            else
            {
                if (reaction.ReactionType == incomingReaction)
                {
                    reaction.ReactionType = ReactionEnum.Null;
                }
                else
                {
                    reaction.ReactionType = incomingReaction;
                }
            }

            await _db.SaveChangesAsync();

            var likes = await _db.BlogsReactions
                .CountAsync(x => x.BlogId == dto.BlogId && x.ReactionType == ReactionEnum.Liked);

            var dislikes = await _db.BlogsReactions
                .CountAsync(x => x.BlogId == dto.BlogId && x.ReactionType == ReactionEnum.Disliked);

            var reactObj = new ReactResponse
            {
                Like = likes,
                Dislike = dislikes,
                UserReaction = (int)reaction.ReactionType
            };

            return Ok(Response<ReactResponse>.Ok(reactObj, "react fetched"));
        }

        [HttpGet("ractions/{blogId}")]
        public async Task<IActionResult> GetReaction(int blogId)
        {
            var likes = await _db.BlogsReactions
                .CountAsync(x => x.BlogId == blogId && x.ReactionType == Enums.ReactionEnum.Liked);

            var dislikes = await _db.BlogsReactions
                .CountAsync(x => x.BlogId == blogId && x.ReactionType == Enums.ReactionEnum.Disliked);

            var reactObj = new ReactResponse
            {
                Like = likes,
                Dislike = dislikes
            };
            return Ok(Response<ReactResponse>.Ok(reactObj, "react fetched"));
        }
    }
}
