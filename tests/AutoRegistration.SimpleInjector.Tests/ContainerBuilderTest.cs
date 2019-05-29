using AutoRegistration.Abstract;
using AutoRegistration.Core.WebApi.SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace AutoRegistration.SimpleInjector.Tests
{
    [TestClass]
    public class ContainerBuilderTest
    {
        private SimpleInjectorContainerBuilder containerBuilder;

        [TestInitialize]
        public void Init()
        {
            var container = new Container();
            containerBuilder = new SimpleInjectorContainerBuilder(container);
        }

        [TestMethod]
        public void CreateTestModelAssembly()
        {
            var registerTimeContainer = containerBuilder.BuildContainer(new[] { GetType().Assembly });

            registerTimeContainer.Verify();
        }
    }
}