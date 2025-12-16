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

        // GET: api/blog
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.Blogs
                .Include(e => e.Category)
                .Include(e => e.ApplicationUser)
                .Where(e => e.BlogVisibility == Enums.BlogVisibility.Public)
                .AsNoTracking()
                .OrderBy(e => e.Id);
                
            var pagedEntities = await PaginatedList<Blog>.CreateAsync(query, pageNumber, pageSize);

            var dtoItems = _mapper.Map<List<BlogResponseDto>>(pagedEntities.Items);

            var result = new
            {
                items = dtoItems,
                pageNumber = pagedEntities.PageIndex,
                pageSize = pagedEntities.PageSize,
                totalCount = pagedEntities.TotalCount,
                totalPages = pagedEntities.TotalPages
            };

            return Ok(Response<object>.Ok(result, "Blogs fetched"));
        }

        [HttpGet("/userblogs/{userId}")]
        public async Task<IActionResult> GetBlogsByUserId(int userId)
        {

            var blogs = await _context.Blogs
                .Include(e => e.Category)
                .Include(e => e.ApplicationUser)
                .Where(e => e.ApplicationUserId == userId).ToListAsync();

            if(blogs == null)
            {
                return NotFound(Response<object>.Fail($"No Blogs Found for user {userId}"));
            }

            var dto = _mapper.Map<List<BlogResponseDto>>(blogs);

            return Ok(Response<List<BlogResponseDto>>.Ok(dto, "Blogs fetched successfully"));

        }

        // GET: api/blog/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Category)
                .Include(b => b.ApplicationUser)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
                return NotFound(Response<string>.Fail("Blog not found"));

            var dto = _mapper.Map<BlogResponseDto>(blog);
            return Ok(Response<BlogResponseDto>.Ok(dto));
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
