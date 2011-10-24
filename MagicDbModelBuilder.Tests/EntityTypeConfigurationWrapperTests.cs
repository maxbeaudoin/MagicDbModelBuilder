using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MagicDbModelBuilder.Tests
{
    [TestFixture]
    public class EntityTypeConfigurationWrapperTests
    {
        private readonly DbModelBuilder _builder;

        public EntityTypeConfigurationWrapperTests()
        {
            _builder = new DbModelBuilder();
        }

        [Test]
        public void HasKey()
        {
            var config = _builder.Entity(typeof (Unicorn))
                .HasKey(typeof (int), "Id");

            Assert.NotNull(config);
            Assert.That(config.GetType() == typeof(EntityTypeConfigurationWrapper));
            Assert.That(config.EntityTypeConfiguration.GetType() == typeof(EntityTypeConfiguration<Unicorn>));
            Assert.That(config.GenericArgument == typeof(Unicorn));
        }

        [Test]
        public void Property()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .Property(typeof (int), "Id");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(PrimitivePropertyConfiguration));
        }

        [Test]
        public void Property_Supports_Nullable()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .Property(typeof (int?), "CornCount");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(PrimitivePropertyConfiguration));
        }

        [Test]
        public void Property_Can_Chain()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .Property(typeof (int), "Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(PrimitivePropertyConfiguration));
        }

        [Test]
        public void DateTimeProperty()
        {
            var property = _builder.Entity(typeof(Unicorn))
                .DateTimeProperty("BornOn");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(DateTimePropertyConfiguration));
        }

        [Test]
        public void DateTimeProperty_Supports_Nullable()
        {
            var property = _builder.Entity(typeof(Unicorn))
                .DateTimeProperty("LastMatedOn", true);

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(DateTimePropertyConfiguration));
        }
    }
}
