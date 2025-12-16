using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.API.Data.DBcontext;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            var dtoList = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(Response<List<CategoryResponseDto>>.Ok(dtoList, "Categories fetched"));
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound(Response<string>.Fail("Category not found"));

            var dto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(Response<CategoryResponseDto>.Ok(dto));
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(Response<CategoryResponseDto>.Ok(responseDto, "Category created"));
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(Response<string>.Fail("Category not found"));

            category.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(Response<string>.Ok("Updated", "Category updated"));
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(Response<string>.Fail("Category not found"));

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(Response<string>.Ok("Deleted", "Category deleted"));
        }
    }
}

