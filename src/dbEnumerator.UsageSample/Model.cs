using Microsoft.EntityFrameworkCore;

namespace dbEnumerator.UsageSample
{
    public class SuperheroDbContext : DbContext
    {
        public SuperheroDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Superhero> Superheros { get; set; }
        public DbSet<ComicEditorCatalogue> ComicEditors { get; set; }
    }

    public class Superhero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ComicEditor ComicEditor { get; set; }
    }

    public enum ComicEditor
    {
        Marvel = 1,
        Dc = 2
    }
    public class ComicEditorCatalogue : EnumBase<ComicEditor> { }
}