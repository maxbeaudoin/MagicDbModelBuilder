using System;
using System.Linq.Expressions;

namespace MagicDbModelBuilder
{
    public class EntityTypeConfigurationWrapper
    {
        private readonly dynamic _config;
        private readonly Type _type;

        public dynamic EntityTypeConfiguration { get { return _config; } }
        public Type GenericArgument { get { return _type; } }

        public EntityTypeConfigurationWrapper(dynamic entityTypeConfiguration)
        {
            _config = entityTypeConfiguration;
            _type = _config.GetType().GetGenericArguments()[0];
        }

        public EntityTypeConfigurationWrapper HasKey(Type keyType, string propertyName)
        {
            var paramEx = Expression.Parameter(_type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);

            _config.GetType()
                .GetMethod("HasKey")
                .MakeGenericMethod(keyType)
                .Invoke(_config, new[] { lambdaEx });

            return _config;
        }
    }
}
