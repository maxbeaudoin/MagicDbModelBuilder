using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using MagicDbModelBuilder.Tests.Model;
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
            Assert.That(config.TypeConfiguration.GetType() == typeof(EntityTypeConfiguration<Unicorn>));
            Assert.That(config.ConfiguredType == typeof(Unicorn));
        }

        [Test]
        public void Ignore()
        {
            _builder.ComplexType(typeof (Corn))
                .Ignore(typeof(int), "BaseRadius");
        }

        [Test]
        public void Property()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .PrimitiveProperty(typeof (int), "Id");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(PrimitivePropertyConfiguration));
        }

        [Test]
        public void Property_Supports_Nullable()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .PrimitiveProperty(typeof(int?), "ChildCount");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(PrimitivePropertyConfiguration));
        }

        [Test]
        public void Property_Can_Chain()
        {
            var property = _builder.Entity(typeof (Unicorn))
                .PrimitiveProperty(typeof(int), "Id")
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
                .NullableDateTimeProperty("LastMatedOn");

            Assert.NotNull(property);
            Assert.That(property.GetType() == typeof(DateTimePropertyConfiguration));
        }
    }
}
