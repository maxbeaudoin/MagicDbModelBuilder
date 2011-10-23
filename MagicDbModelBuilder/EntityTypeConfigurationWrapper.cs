using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public EntityTypeConfigurationWrapper HasKey(Type keyType, params string[] propertyNames)
        {
            if (propertyNames.Length > 1)
                throw new NotImplementedException();

            var paramEx = Expression.Parameter(_type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyNames[0]), paramEx);

            _config.GetType()
                .GetMethod("HasKey")
                .MakeGenericMethod(keyType)
                .Invoke(_config, new[] { lambdaEx });

            return this;
        }

        public PrimitivePropertyConfiguration Property(Type propertyType, string propertyName)
        {
            var typeIsNullable = propertyType.IsGenericType &&
                                 propertyType.GetGenericTypeDefinition() == typeof (Nullable<>);

            MethodInfo[] methods = _config.GetType().GetMethods();
            var candidates = methods.Where(m => m.Name == "Property" && m.IsGenericMethod);

            var property = candidates.First(m =>
            {
                var param = m.GetParameters()[0];
                var type = param.ParameterType;             // Expression<>
                var func = type.GetGenericArguments()[0];   // Func<,>
                var T = func.GetGenericArguments()[1];      // T or T?
                var isNullable = T.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                return (typeIsNullable && isNullable) || (!typeIsNullable && !isNullable);
            });
            
            var paramEx = Expression.Parameter(_type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);

            var nonNullableType = typeIsNullable ? propertyType.GetGenericArguments()[0] : propertyType;

            return property
                .MakeGenericMethod(nonNullableType)
                .Invoke(_config, new[] { lambdaEx });
        }

        public DateTimePropertyConfiguration DateTimeProperty(string propertyName, bool nullable = false)
        {
            var datetimeType = nullable ? typeof (DateTime?) : typeof(DateTime);

            var exType = typeof(Expression<>)
                .MakeGenericType(typeof(Func<,>)
                .MakeGenericType(_type, datetimeType));

            var paramEx = Expression.Parameter(_type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);


            return _config.GetType()
                .GetMethod("Property", new[] { exType })
                .Invoke(_config, new[] { lambdaEx });
        }
    }
}
