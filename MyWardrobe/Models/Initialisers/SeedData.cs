using Microsoft.EntityFrameworkCore;
using MyWardrobe.Data;

namespace MyWardrobe.Models.Initialisers;

// Based this code from a learn.microsoft.com tutorial
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-9.0&tabs=visual-studio
public static class SeedData
{
    // Body Locations Reference Constants
    private static readonly string _torso = "Torso";
    private static readonly string _wrist = "Wrist";
    private static readonly string _legs = "Legs";
    private static readonly string _feet = "Feet";

    // Brand Reference Constants
    private static readonly string _nike = "Nike";
    private static readonly string _adidas = "Adidas";
    private static readonly string _levis = "Levi's";
    private static readonly string _hAndM = "H&M";
    private static readonly string _uniqlo = "Uniqlo";
    private static readonly string _casio = "Casio";

    // Clothing Category Reference Constants
    private static readonly string _tshirt = "T-Shirt";
    private static readonly string _watch = "Watch";
    private static readonly string _jacket = "Jacket";
    private static readonly string _jeans = "Jeans";
    private static readonly string _runningShoes = "Running Shoes";
    private static readonly string _boots = "Boots";

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

            // Clothing items relies on both categories and brands having been generated first
            List<ClothingItem> seedClothingItems = createSeedClothingItems(seedCategories, seedBrands);
            context.ClothingItem.AddRange(seedClothingItems);

            context.SaveChanges();
        }
    }

    private static List<ClothingItem> createSeedClothingItems(List<Category> seedCategories, List<Brand> seedBrands)
    {
        List<ClothingItem> seedClothingItems = new();

        Category watch = seedCategories.Where(x => x.Name == _watch).FirstOrDefault()!;
        Category runningShoe = seedCategories.Where(x => x.Name == _runningShoes).FirstOrDefault()!;

        Brand nike = seedBrands.Where(x => x.Name == _nike).FirstOrDefault()!;
        Brand adidas = seedBrands.Where(x => x.Name == _adidas).FirstOrDefault()!;
        Brand casio = seedBrands.Where(x => x.Name == _casio).FirstOrDefault()!;

        // Add example watches
        seedClothingItems.Add(new ClothingItem
        {
            CategoryId = watch.Id,
            Category = watch,
            BrandId = casio.Id,
            Brand = casio,
        });
        seedClothingItems.Add(new ClothingItem
        {
            CategoryId = watch.Id,
            Category = watch,
            BrandId = nike.Id,
            Brand = nike,
        });

        // Add example running shoes
        seedClothingItems.Add(new ClothingItem
        {
            CategoryId = runningShoe.Id,
            Category = runningShoe,
            BrandId = nike.Id,
            Brand = nike,
        });
        seedClothingItems.Add(new ClothingItem
        {
            CategoryId = runningShoe.Id,
            Category = runningShoe,
            BrandId = adidas.Id,
            Brand = adidas,
        });

        return seedClothingItems;
    }

    private static List<WearLocation> createSeedWearLocations()
    {
        List<WearLocation> seedWearLocations = new();

        seedWearLocations.Add(new WearLocation { Name = _torso });
        seedWearLocations.Add(new WearLocation { Name = _wrist });
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
            Name = _tshirt,
            WearLocationId = torso.Id,
            WearLocation = torso
        });
        seedCategories.Add(new Category
        {
            Name = _jacket,
            WearLocationId = torso.Id,
            WearLocation = torso
        });

        WearLocation wrists = seedWearLocations.Where(x => x.Name == _wrist).FirstOrDefault()!;
        seedCategories.Add(new Category
        {
            Name = _watch,
            WearLocationId = wrists.Id,
            WearLocation = wrists
        });

        WearLocation legs = seedWearLocations.Where(x => x.Name == _legs).FirstOrDefault()!;
        seedCategories.Add(new Category
        {
            Name = _jeans,
            WearLocationId = legs.Id,
            WearLocation = legs
        });

        WearLocation feet = seedWearLocations.Where(x => x.Name == _feet).FirstOrDefault()!;
        seedCategories.Add(new Category
        {
            Name = _boots,
            WearLocationId = feet.Id,
            WearLocation = feet
        });
        seedCategories.Add(new Category
        {
            Name = _runningShoes,
            WearLocationId = feet.Id,
            WearLocation = feet
        });

        return seedCategories;
    }

    private static List<Brand> createSeedBrands()
    {
        List<Brand> seedBrands = new();

        seedBrands.Add(new Brand { Name = _nike });
        seedBrands.Add(new Brand { Name = _adidas });
        seedBrands.Add(new Brand { Name = _levis });
        seedBrands.Add(new Brand { Name = _hAndM });
        seedBrands.Add(new Brand { Name = _uniqlo });
        seedBrands.Add(new Brand { Name = _casio });

        return seedBrands;
    }
}