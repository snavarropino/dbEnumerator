using System;
using System.Threading.Tasks;
using dbEnumerator.Test.Infrastructure;
using dbEnumerator.Test.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace dbEnumerator.Test
{
    [Collection("Database collection")]
    public class dbEnumeratorShould
    {
        private readonly DatabaseFixture _fixture;

        public dbEnumeratorShould(DatabaseFixture fixture)
        {
            _fixture = fixture;
            Task.Run(async () => { await _fixture.ResetDatabase(); }).Wait();
        }

        [Fact]
        public async Task populate_all_enum_values_using_dbset()
        {
            using (var context = _fixture.GetDbContext())
            {
                await EnumBasedEntitySeeder.SeedEntityAsync<ComicEditorCatalogue, ComicEditor>(context.ComicEditors);
                await context.SaveChangesAsync();

                (await context.ComicEditors.CountAsync())
                    .Should()
                    .Be(Enum.GetValues(typeof(ComicEditor)).Length);
            }
        }

        [Fact]
        public async Task populate_enum_descriptions_using_dbset()
        {
            using (var context = _fixture.GetDbContext())
            {
                await EnumBasedEntitySeeder.SeedEntityAsync<ComicEditorCatalogue, ComicEditor>(context.ComicEditors);
                await context.SaveChangesAsync();

                (await context.ComicEditors.SingleAsync(c => c.Id == (int) ComicEditor.Dc)).Description
                    .Should()
                    .Be(TestUtils.GetEnumDescription(ComicEditor.Dc));
            }
        }

        [Fact]
        public void give_a_name_to_foreign_key_if_backing_field_starts_with_hyphen()
        {
            EnumBasedEntityConfigurator<object, object>.GetColumnName("_field")
                .Should().Be("Field");
        }

        [Fact]
        public void give_a_name_to_foreign_key_if_backing_field_does_not_start_with_hyphen()
        {
            EnumBasedEntityConfigurator<object, object>.GetColumnName("field")
                .Should().Be("Field");
        }
        
        public enum BadComicEditor
        {
            Marvel = 0,
            Dc = 1
        }

        public class BadComicEditorCatalogue : EnumBasedEntity<BadComicEditor> { }
    }
}