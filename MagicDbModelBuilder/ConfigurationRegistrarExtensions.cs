using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MagicDbModelBuilder
{
    public static class ConfigurationRegistrarExtensions
    {
        public static void Scan(this ConfigurationRegistrar configurationRegistrar, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            
            foreach(var t in types)
            {
                dynamic config = Activator.CreateInstance(t);
                configurationRegistrar.Add(config);
            }
        }

        public static bool IsConfigured<T>(this ConfigurationRegistrar configurationRegistrar)
        {
            return configurationRegistrar.IsConfigured(typeof(T));
        }

        public static bool IsConfigured(this ConfigurationRegistrar configurationRegistrar, Type type)
        {
            var configuredTypes = (IEnumerable<Type>)configurationRegistrar
                .GetType()
                .GetMethod("GetConfiguredTypes", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(configurationRegistrar, null);
            return configuredTypes.Any(t => t == type);
        }
    }
}
