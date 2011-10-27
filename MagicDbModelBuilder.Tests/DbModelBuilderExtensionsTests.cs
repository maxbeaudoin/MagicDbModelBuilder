using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using MagicDbModelBuilder.Tests.Model;
using NUnit.Framework;

namespace MagicDbModelBuilder.Tests
{
    [TestFixture]
    public class DbModelBuilderExtensionsTests
    {
        private readonly DbModelBuilder _builder;

        public DbModelBuilderExtensionsTests()
        {
            _builder = new DbModelBuilder();
        }

        [Test]
        public void Entity()
        {
            var config = _builder.Entity(typeof(Unicorn));
            
            Assert.NotNull(config);
            Assert.That(config.GetType() == typeof(EntityTypeConfigurationWrapper));
            Assert.That(config.TypeConfiguration.GetType() == typeof(EntityTypeConfiguration<Unicorn>));
            Assert.That(config.ConfiguredType == typeof(Unicorn));

            Assert.That(_builder.Configurations.IsConfigured(typeof(Unicorn)));
        }

        [Test]
        public void ComplexType()
        {
            var config = _builder.ComplexType(typeof(Corn));

            Assert.NotNull(config);
            Assert.That(config.GetType() == typeof(ComplexTypeConfigurationWrapper));
            Assert.That(config.TypeConfiguration.GetType() == typeof(ComplexTypeConfiguration<Corn>));
            Assert.That(config.ConfiguredType == typeof(Corn));

            Assert.That(_builder.Configurations.IsConfigured(typeof(Corn)));
        }
    }
}
