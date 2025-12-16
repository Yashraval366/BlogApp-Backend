using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.API.Data.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() : base()
        {
            
        }
        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }
}
