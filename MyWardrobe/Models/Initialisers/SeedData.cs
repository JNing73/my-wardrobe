using Microsoft.EntityFrameworkCore;
using MyWardrobe.Data;

namespace MyWardrobe.Models.Initialisers;

// Based this code from a learn.microsoft.com tutorial
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-9.0&tabs=visual-studio
public static class SeedData
{
    // define constants for body location names
    private static readonly string _torso = "Torso";
    private static readonly string _legs = "Legs";
    private static readonly string _feet = "Feet";

    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MyWardrobeContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MyWardrobeContext>>()))
        {
            if (context.Category.Any()
                || context.Brand.Any()
                || context.WearLocation.Any()
                || context.ClothingItem.Any())
            {
                return;   // DB has been seeded
            }

            // First seed models which have no foreign keys
            List<WearLocation> seedWearLocations = createSeedWearLocations();
            context.AddRange(seedWearLocations);
            context.SaveChanges();

            // Note: Wear Locations need to be seeded before Categories can be declared
            // because Wear Location is a required field
            List<Category> seedCategories = createSeedCategories(seedWearLocations);
            List<Brand> seedBrands = createSeedBrands();

            context.Category.AddRange(seedCategories);
            context.Brand.AddRange(seedBrands);

            context.SaveChanges();
        }
    }

    private static List<WearLocation> createSeedWearLocations()
    {
        List<WearLocation> seedWearLocations = new();

        seedWearLocations.Add(new WearLocation { Name = _torso });
        seedWearLocations.Add(new WearLocation { Name = _legs });
        seedWearLocations.Add(new WearLocation { Name = _feet });

        return seedWearLocations;
    }

    private static List<Category> createSeedCategories(List<WearLocation> seedWearLocations)
    {
        List<Category> seedCategories = new();

        WearLocation torso = seedWearLocations.Where(x => x.Name == _torso).FirstOrDefault()!;
        seedCategories.Add(new Category
        {
            Name = "T-Shirt",
            WearLocationId = torso.Id,
            WearLocation = torso
        });

        WearLocation legs = seedWearLocations.Where(x => x.Name == _legs).FirstOrDefault()!;
        seedCategories.Add(new Category
        {
            Name = "Jeans",
            WearLocationId = legs.Id,
            WearLocation = legs
        });

        return seedCategories;
    }

    private static List<Brand> createSeedBrands()
    {
        List<Brand> seedBrands = new();

        seedBrands.Add(new Brand { Name = "Nike" });
        seedBrands.Add(new Brand { Name = "Adidas" });

        return seedBrands;
    }
}