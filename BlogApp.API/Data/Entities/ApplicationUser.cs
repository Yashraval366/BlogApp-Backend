using Microsoft.AspNetCore.Identity;

namespace BlogApp.API.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }

        public ICollection<Blog>? Blogs { get; set; }

        public ICollection<BlogReaction>? Reactions { get; set; }

        public ICollection<Comment>? Comments { get; set; }

    }
}
