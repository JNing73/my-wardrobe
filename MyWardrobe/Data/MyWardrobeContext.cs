using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWardrobe.Models;

namespace MyWardrobe.Data
{
    public class MyWardrobeContext : DbContext
    {
        public MyWardrobeContext (DbContextOptions<MyWardrobeContext> options)
            : base(options)
        {
        }

        public DbSet<MyWardrobe.Models.WearLocation> WearLocation { get; set; } = default!;
        public DbSet<MyWardrobe.Models.Category> Category { get; set; } = default!;
        public DbSet<MyWardrobe.Models.Brand> Brand { get; set; } = default!;
        public DbSet<MyWardrobe.Models.ClothingItem> ClothingItem { get; set; } = default!;
    }
}
