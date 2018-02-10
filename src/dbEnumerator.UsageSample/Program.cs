using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dbEnumerator.UsageSample
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var optionsBuilder = new DbContextOptionsBuilder<SuperheroDbContext>();
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=dbEnumeratorSuperheros;Trusted_Connection=True;");

            using (var ctx = new SuperheroDbContext(optionsBuilder.Options))
            {
                ctx.Database.EnsureCreated();
                await EnumSeeder.SeedEnumDataAsync<ComicEditorCatalogue, ComicEditor>(ctx.ComicEditors);
                await ctx.SaveChangesAsync();
                Console.WriteLine("Database create and seeded...");

                await ctx.AddAsync(new Superhero() {Name = "Mento", Age = 30, ComicEditor = ComicEditor.Dc});
                await ctx.SaveChangesAsync();

                var mento = await ctx.Superheros.FirstOrDefaultAsync();
                Console.WriteLine($"Readed {mento.Name}, avaliable in {mento.ComicEditor} comics");
            }
        }
    }
}