using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace dbEnumerator
{
    public  class EnumBasedEntityConfigurator<TEntity, TRelatedEntity> 
        where TEntity:class  
        where TRelatedEntity: class
    {
        public static void Register(ModelBuilder modelBuilder,
            Expression<Func<TEntity, object>> ignore,
            string backingField,
            Expression<Func<TEntity, TRelatedEntity>> relation)
        {
            var backingFieldName = backingField;

            modelBuilder.Entity<TEntity>()
                .Ignore(ignore)
                .Property<int>(backingFieldName)
                .HasColumnName(GetColumnName(backingFieldName))
                .IsRequired();

            modelBuilder.Entity<TEntity>()
                .HasOne(relation)
                .WithMany()
                .HasForeignKey(backingFieldName)
                .IsRequired();
        }

        public static string GetColumnName(string backingFieldName)
        {
            var field = RemoveStartingHyphen(backingFieldName);
            return $"{field.First().ToString().ToUpper()}{field.Substring(1)}";
        }

        private static string RemoveStartingHyphen(string backingFieldName)
        {
            if (backingFieldName.StartsWith("_"))
                return backingFieldName.Substring(1);

            return backingFieldName;
        }
    }
}