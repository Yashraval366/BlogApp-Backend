using System;
using System.Security.Claims;
using AutoMapper;
using BlogApp.API.Data.DBcontext;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using BlogApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlogController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/blog
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = _context.Blogs
                .Where(e => e.BlogVisibility == Enums.BlogVisibility.Public)
                .Select(e => new BlogResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,

                    AuthorId = e.ApplicationUserId,
                    AuthorName = e.ApplicationUser!.FullName,

                    CategoryId = e.CategoryId,
                    CategoryName = e.Category!.Name,

                    BlogVisibility = e.BlogVisibility,

                    LikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Liked),
                    DislikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Disliked),

                    CommentCounts = e.Comments!.Count(c => !c.IsDeleted),

                    UserReaction = e.Reactions
                            .Where(w => w.UserId == userId)
                            .Select(r => (int?)r.ReactionType)
                            .FirstOrDefault(),

                    CreatedAt = e.CreatedAt,
                    LastUpdatedAt = e.LastUpdatedAt

                })
                .AsNoTracking()
                .OrderBy(e => e.Id);

            var pagedEntities = await PaginatedList<BlogResponseDto>.CreateAsync(query, pageNumber, pageSize);

            var result = new
            {
                items = pagedEntities.Items,
                pageNumber = pagedEntities.PageIndex,
                pageSize = pagedEntities.PageSize,
                totalCount = pagedEntities.TotalCount,
                totalPages = pagedEntities.TotalPages
            };

            return Ok(Response<object>.Ok(result, "Blogs fetched"));
        }

        [HttpGet("userblogs/{userId}")]
        public async Task<IActionResult> GetBlogsByUserId(int userId,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var query = _context.Blogs
                .Where(e => e.ApplicationUserId == userId)
                .Select(e => new BlogResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,

                    AuthorId = e.ApplicationUserId,
                    AuthorName = e.ApplicationUser.FullName,

                    CategoryId = e.CategoryId,
                    CategoryName = e.Category!.Name,

                    BlogVisibility = e.BlogVisibility,

                    LikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Liked),
                    DislikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Disliked),
                    CommentCounts = e.Comments!.Count(c => !c.IsDeleted),

                    UserReaction = e.Reactions
                            .Where(w => w.UserId == userId)
                            .Select(r => (int?)r.ReactionType)
                            .FirstOrDefault(),

                    CreatedAt = e.CreatedAt,
                    LastUpdatedAt = e.LastUpdatedAt
                })
                .AsNoTracking()
                .OrderBy(e => e.Id);

            var pagedEntities = await PaginatedList<BlogResponseDto>.CreateAsync(query, pageNumber, pageSize);

            var result = new
            {
                items = pagedEntities.Items,
                pageNumber = pagedEntities.PageIndex,
                pageSize = pagedEntities.PageSize,
                totalCount = pagedEntities.TotalCount,
                totalPages = pagedEntities.TotalPages
            };

            if (result.items.Count == 0)
            {
                return Ok(Response<object>.Fail("No Blogs Found"));
            }

            return Ok(Response<object>.Ok(result, "Blogs fetched"));

        }

        // GET: api/blog/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var blog = await _context.Blogs
                .Where(e => e.Id == id)
                .Select(e => new BlogResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,

                    AuthorId = e.ApplicationUserId,
                    AuthorName = e.ApplicationUser.FullName,

                    CategoryId = e.CategoryId,
                    CategoryName = e.Category!.Name,

                    BlogVisibility = e.BlogVisibility,

                    LikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Liked),
                    DislikeCounts = e.Reactions!.Count(r => r.ReactionType == Enums.ReactionEnum.Disliked),
                    CommentCounts = e.Comments!.Count(c => !c.IsDeleted),

                    UserReaction = e.Reactions
                            .Where(w => w.UserId == userId)
                            .Select(r => (int?)r.ReactionType)
                            .FirstOrDefault(),

                    CreatedAt = e.CreatedAt,
                    LastUpdatedAt = e.LastUpdatedAt
                })
                .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound(Response<string>.Fail("Blog not found"));
          
            return Ok(Response<BlogResponseDto>.Ok(blog,"blog fetched"));
        }

        // POST: api/blog
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateDto dto)
        {
            var blog = _mapper.Map<Blog>(dto);

            blog.ApplicationUserId = int.Parse(User.FindFirst((ClaimTypes.NameIdentifier))!.Value);
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BlogResponseDto>(blog);
            return Ok(Response<BlogResponseDto>.Ok(result, "Blog created"));
        }

        // PUT: api/blog/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BlogUpdateDto dto)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(e => e.Id == id);
            if (blog == null)
                return NotFound(Response<string>.Fail("Blog not found"));

            _mapper.Map(dto, blog);
            blog.LastUpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(Response<string>.Ok("Updated", "Blog updated"));
        }

        // DELETE: api/blog/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound(Response<string>.Fail("Blog not found"));

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok(Response<string>.Ok("Deleted", "Blog deleted"));
        }
    }
}
