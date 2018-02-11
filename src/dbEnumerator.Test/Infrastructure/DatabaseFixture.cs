using System.Threading.Tasks;
using dbEnumerator.Test.Model;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace dbEnumerator.Test.Infrastructure
{
    public class DatabaseFixture
    {
        const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=dbEnumeratorSuperheroTest;Trusted_Connection=True;";
        private static readonly Checkpoint _checkpoint = new Checkpoint();

        public DatabaseFixture()
        {
            EnsureDatabase();
        }

        public async Task ResetDatabase()
        {
            await _checkpoint.Reset(ConnectionString);
        }

        public SuperheroDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SuperheroDbContext>();
            optionsBuilder.UseSqlServer(
                ConnectionString);

            return new SuperheroDbContext(optionsBuilder.Options);
        }

        private void EnsureDatabase()
        {
            using (var context = GetDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}