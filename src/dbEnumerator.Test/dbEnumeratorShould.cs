using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using dbEnumerator.Test.Infrastructure;
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
            _fixture.ResetDatabase().GetAwaiter();
        }

        [Fact]
        public async Task populate_all_enum_values_using_dbset()
        {
            using (var context = _fixture.GetDbContext())
            {
                await EnumSeeder.SeedEnumDataAsync<ComicEditorCatalogue, ComicEditor>(context.ComicEditors);
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
                await EnumSeeder.SeedEnumDataAsync<ComicEditorCatalogue, ComicEditor>(context.ComicEditors);
                await context.SaveChangesAsync();

                (await context.ComicEditors.SingleAsync(c => c.Id == (int) ComicEditor.Dc)).Description
                    .Should()
                    .Be(TestUtils.GetEnumDescription(ComicEditor.Dc));
            }
        }
    }
    
    public static class TestUtils
    {
        public static string GetEnumDescription<TEnum>(TEnum item)
        {
            var attribute = item.GetType().GetField(item.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute?.Description ?? string.Empty;
        }
    }
}