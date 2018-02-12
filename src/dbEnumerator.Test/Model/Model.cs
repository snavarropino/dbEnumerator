using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace dbEnumerator.Test.Model
{
    public class SuperheroDbContext : DbContext
    {
        public SuperheroDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Superhero> Superheros { get; set; }
        public DbSet<ComicEditorCatalogue> ComicEditors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Superhero>()
                .Ignore(s => s.ComicEditor)      // Ignore enum property
                .Property<int>("_comicEditorId")   // Define backing field with no property
                .HasColumnName("ComicEditorId")       // Set proper column name for foreign key
                .IsRequired();

            modelBuilder.Entity<Superhero>()
                .HasOne(s => s.ComicEditorCatalogue)
                .WithMany()
                .HasForeignKey("_comicEditorId") // Set foreign key to backing field
                .IsRequired();
        }
    }

    public class Superhero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ComicEditor ComicEditor
        {
            get => (ComicEditor)_comicEditorId;
            set => _comicEditorId = (int)value;
        }

        public ComicEditorCatalogue ComicEditorCatalogue { get; set; }

        private int _comicEditorId;
    }

    public enum ComicEditor
    {
        Marvel = 1,
        [Description("Dc comics")]
        Dc = 2
    }
    public class ComicEditorCatalogue : EnumBase<ComicEditor> { }
}