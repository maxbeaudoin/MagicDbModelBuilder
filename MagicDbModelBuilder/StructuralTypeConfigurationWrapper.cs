using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Spatial;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MagicDbModelBuilder
{
    public abstract class StructuralTypeConfigurationWrapper
    {
        protected readonly dynamic Config;
        protected readonly Type Type;

        public dynamic TypeConfiguration { get { return Config; } }
        public Type ConfiguredType { get { return Type; } }

        protected StructuralTypeConfigurationWrapper(dynamic typeConfiguration)
        {
            Config = typeConfiguration;
            Type = typeConfiguration.GetType().GetGenericArguments()[0];
        }

        public void Ignore(Type propertyType, string propertyName)
        {
            var paramEx = Expression.Parameter(Type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);
            
            Config.GetType()
                .GetMethod("Ignore")
                .MakeGenericMethod(propertyType)
                .Invoke(Config, new[] { lambdaEx });
        }

        public dynamic DynamicProperty(Type propertyType, string propertyName)
        {
            var exType = typeof(Expression<>)
                .MakeGenericType(typeof(Func<,>)
                .MakeGenericType(Type, propertyType));

            var paramEx = Expression.Parameter(Type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);


            return Config.GetType()
                .GetMethod("Property", new[] { exType })
                .Invoke(Config, new[] { lambdaEx });
        }

        public PrimitivePropertyConfiguration PrimitiveProperty(Type propertyType, string propertyName)
        {
            var typeIsNullable = propertyType.IsGenericType &&
                                 propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            MethodInfo[] methods = Config.GetType().GetMethods();
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

            var paramEx = Expression.Parameter(Type, "x");
            var lambdaEx = Expression.Lambda(Expression.Property(paramEx, propertyName), paramEx);

            var nonNullableType = typeIsNullable ? propertyType.GetGenericArguments()[0] : propertyType;

            return property
                .MakeGenericMethod(nonNullableType)
                .Invoke(Config, new[] { lambdaEx });
        }
        
        public DateTimePropertyConfiguration DateTimeProperty(string propertyName)
        {
            return DynamicProperty(typeof (DateTime), propertyName);
        }

        public DateTimePropertyConfiguration NullableDateTimeProperty(string propertyName)
        {
            return DynamicProperty(typeof (DateTime?), propertyName);
        }

        public StringPropertyConfiguration StringProperty(string propertyName)
        {
            return DynamicProperty(typeof(string), propertyName);
        }

        public BinaryPropertyConfiguration BinaryProperty(string propertyName)
        {
            return DynamicProperty(typeof(byte[]), propertyName);
        }

        public DecimalPropertyConfiguration DecimalProperty(string propertyName)
        {
            return DynamicProperty(typeof(decimal), propertyName);
        }

        public DecimalPropertyConfiguration NullableDecimalProperty(string propertyName)
        {
            return DynamicProperty(typeof(Decimal?), propertyName);
        }

        public DateTimePropertyConfiguration DateTimeOffsetProperty(string propertyName)
        {
            return DynamicProperty(typeof(DateTimeOffset), propertyName);
        }

        public DateTimePropertyConfiguration NullableDateTimeOffsetProperty(string propertyName)
        {
            return DynamicProperty(typeof(DateTimeOffset?), propertyName);
        }

        public DateTimePropertyConfiguration TimeSpanProperty(string propertyName)
        {
            return DynamicProperty(typeof(TimeSpan), propertyName);
        }

        public DateTimePropertyConfiguration NullableTimeSpanProperty(string propertyName)
        {
            return DynamicProperty(typeof(TimeSpan?), propertyName);
        }
    }
}
