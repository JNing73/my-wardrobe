using System.ComponentModel.DataAnnotations;

namespace MyWardrobe.Models;

public class Brand
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

}
