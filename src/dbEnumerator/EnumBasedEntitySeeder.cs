using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dbEnumerator
{
    public class EnumBasedEntitySeeder
    {
        public static async Task SeedEntityAsync<TEntity, TEnum>(DbSet<TEntity> items)
            where TEntity : EnumBasedEntity<TEnum>
            where TEnum : struct
        {
            var enumType = EnumHelper.EnsureEnum<TEnum>();

            foreach (TEnum evalue in Enum.GetValues(enumType))
            {
                var id = (int)Convert.ChangeType(evalue, typeof(int));

                if (id <= 0)
                    throw new ArgumentException("Underlying enum value must start with 1");

                if (!items.Any(a => a.Id == id))
                {
                    var item = Activator.CreateInstance<TEntity>();
                    item.Id = id;
                    item.Name = Enum.GetName(enumType, evalue);
                    item.Description = EnumHelper.GetEnumDescription(evalue);
                    await items.AddAsync(item);
                }
            }
        }

    }
}