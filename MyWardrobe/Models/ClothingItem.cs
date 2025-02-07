using System.ComponentModel.DataAnnotations;

namespace MyWardrobe.Models
{
    public class ClothingItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public required int CategoryId { get; set; }
        public Category? Category { get; set; }


        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }


        [MaxLength(100)]
        public string? Size { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public string? ImageFileName { get; set; }
    }
}
