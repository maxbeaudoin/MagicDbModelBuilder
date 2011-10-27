using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MagicDbModelBuilder
{
    public class EntityTypeConfigurationWrapper: StructuralTypeConfigurationWrapper
    {
        public EntityTypeConfigurationWrapper(dynamic entityTypeConfiguration)
            : base((object)entityTypeConfiguration)
        {
        }

        public EntityTypeConfigurationWrapper HasKey(Type keyType, params string[] propertyNames)
        {
            if (propertyNames.Length > 1)
                throw new NotImplementedException();

            var paramEx = Expression.Parameter(Type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyNames[0]), paramEx);

            Config.GetType()
                .GetMethod("HasKey")
                .MakeGenericMethod(keyType)
                .Invoke(Config, new[] { lambdaEx });

            return this;
        }
    }
}
