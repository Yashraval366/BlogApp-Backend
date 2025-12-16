using System.Runtime.InteropServices;
using AutoMapper;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogApp.API.AutoMapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {

            CreateMap<Blog, BlogResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.ApplicationUserId));

            CreateMap<BlogCreateDto, Blog>();
            CreateMap<BlogUpdateDto, Blog>();




        }
    }
}
