using System;
using System.Data.Entity;

namespace MagicDbModelBuilder
{
    public class DbModelBuilderWrapper
    {
        private readonly dynamic _builder;

        public dynamic DbModelBuilder { get { return _builder; } }

        public DbModelBuilderWrapper(DbModelBuilder builder)
        {
            _builder = builder;
        }
        
        public EntityTypeConfigurationWrapper Entity(Type entityType)
        {
            return _builder.GetType()
                .GetMethod("Entity")
                .MakeGenericMethod(entityType)
                .Invoke(_builder, null);
        }
    }
}
