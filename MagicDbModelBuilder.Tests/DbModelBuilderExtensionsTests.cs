using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
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
            Assert.That(config.EntityTypeConfiguration.GetType() == typeof(EntityTypeConfiguration<Unicorn>));
            Assert.That(config.GenericArgument == typeof(Unicorn));
        }
    }
}
