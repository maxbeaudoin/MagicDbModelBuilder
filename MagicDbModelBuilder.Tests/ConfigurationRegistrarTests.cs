using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using MagicDbModelBuilder.Tests.Model;
using NUnit.Framework;

namespace MagicDbModelBuilder.Tests
{
    [TestFixture]
    public class ConfigurationRegistrarTests
    {
        private readonly DbModelBuilder _builder;

        public ConfigurationRegistrarTests()
        {
            _builder = new DbModelBuilder();
        }

        [Test]
        public void Scan_By_Assembly()
        {
            _builder.Configurations
                .Scan(Assembly.GetExecutingAssembly());

            Assert.That(_builder.Configurations.IsConfigured<Unicorn>());
            Assert.That(_builder.Configurations.IsConfigured(typeof(Unicorn)));
        }
    }
}
