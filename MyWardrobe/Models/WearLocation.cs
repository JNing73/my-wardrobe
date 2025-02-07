using System.ComponentModel.DataAnnotations;

namespace MyWardrobe.Models
{
    public class WearLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public ICollection<Category>? ClothingCategories { get; set; }
    }
}
