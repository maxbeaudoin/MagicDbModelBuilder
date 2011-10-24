using System;
using System.Data.Entity;

namespace MagicDbModelBuilder
{
    public static class DbModelBuilderExtensions
    {
        public static EntityTypeConfigurationWrapper Entity(this DbModelBuilder builder, Type entityType)
        {
            var config = builder.GetType()
                .GetMethod("Entity")
                .MakeGenericMethod(entityType)
                .Invoke(builder, null);
            return new EntityTypeConfigurationWrapper(config);
        }
    }
}
