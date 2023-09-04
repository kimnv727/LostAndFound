using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LostAndFound.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
            Expression<Func<TInterface, bool>> expression)
        {
            var entityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);

            foreach (var type in entityTypes)
            {
                var newParam = Expression.Parameter(type);
                var newbody =
                    ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(type).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }

        public static void SaveEnumAsString<T>(this ModelBuilder modelBuilder) where T : Enum
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType))
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.GetType() == typeof(T))
                    {
                        modelBuilder.Entity(type).Property(prop.Name).HasConversion<string>();
                    }
                }
            }
        }

        public static void SaveEnumsAsString(this ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType))
            {
                foreach (var prop in type.GetProperties())
                {
                    if (typeof(Enum).IsAssignableFrom(prop.GetType()))
                    {
                        modelBuilder.Entity(type).Property(prop.Name).HasConversion<string>();
                    }
                }
            }
        }
    }
}
