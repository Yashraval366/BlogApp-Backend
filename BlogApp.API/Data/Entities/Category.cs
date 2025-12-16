using System.ComponentModel.DataAnnotations;

namespace BlogApp.API.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
