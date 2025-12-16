using Microsoft.AspNetCore.Identity;

namespace BlogApp.API.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }

        ICollection<Blog>? Blogs { get; set; }

    }
}
