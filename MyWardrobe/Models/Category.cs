using System.ComponentModel.DataAnnotations;

namespace MyWardrobe.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required int WearLocationId { get; set; }
        public WearLocation? WearLocation { get; set; }

    }
}
