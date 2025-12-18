
using BlogApp.API.Enums;

namespace BlogApp.API.DTOs.Response
{
    public class ReactResponse
    {
        public int Like { get; set; }

        public int Dislike { get; set; }

        public int UserReaction { get; set; }

    }
}
