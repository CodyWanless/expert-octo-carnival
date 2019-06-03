using System;
using System.Collections.Generic;
using System.Linq;
using AutoRegistration.Abstract;
using AutoRegistration.Core.WebApi.SimpleInjector;
using AutoRegistration.SimpleInjector.Tests.TestModel;
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

        [TestMethod]
        public void CreateTestModelAssembly_BarRepository()
        {
            var registerTimeContainer = containerBuilder.BuildContainer(new[] { GetType().Assembly });

            var serviceProvider = registerTimeContainer.ToRuntimeContainer();
            var barRepo = serviceProvider.GetService(typeof(IRepository<BarModel>));

            Assert.IsInstanceOfType(barRepo, typeof(BarRepository));
        }

        [TestMethod]
        public void CreateTestModelAssembly_StringValidatorCheck()
        {
            var registerTimeContainer = containerBuilder.BuildContainer(new[] { GetType().Assembly });

            var serviceProvider = registerTimeContainer.ToRuntimeContainer();
            var validators = (IEnumerable<IValidator<string>>)serviceProvider.GetService(typeof(IEnumerable<IValidator<string>>));

            var expected = new HashSet<Type>(
                new[]
                {
                    typeof(AStringValidator),
                    typeof(NullValidator<string>),
                    typeof(StringEmptyValidator),
                    typeof(TrueValidator<string>)
                });

            Assert.IsTrue(expected.SetEquals(validators.Select(v => v.GetType())));
        }

        [TestMethod]
        public void CreateTestModelAssembly_IntValidatorCheck()
        {
            var registerTimeContainer = containerBuilder.BuildContainer(new[] { GetType().Assembly });

            var serviceProvider = registerTimeContainer.ToRuntimeContainer();
            var validators = (IEnumerable<IValidator<int>>)serviceProvider.GetService(typeof(IEnumerable<IValidator<int>>));

            var expected = new HashSet<Type>(
                new[]
                {
                    typeof(NullValidator<int>),
                    typeof(TrueValidator<int>)
                });

            Assert.IsTrue(expected.SetEquals(validators.Select(v => v.GetType())));
        }

        [TestMethod]
        public void CreateTestModelAssembly_RepoSkipConvention()
        {
            var registerTimeContainer = containerBuilder.BuildContainer(new[] { GetType().Assembly },
                new[] { new SkipBarRepoRegistrations() });

            var serviceProvider = registerTimeContainer.ToRuntimeContainer();
            var barRepo = serviceProvider.GetService(typeof(IRepository<BarModel>));

            Assert.IsNull(barRepo);
        }

        private class SkipBarRepoRegistrations : IRegistrationConvention
        {
            public IEnumerable<Type> InterfacesToRegister =>
                new[]
                {
                    typeof(IRepository<BarModel>)
                };

            public IRegisterTimeContainer Register(IReadOnlyCollection<Type> types, IRegisterTimeContainer container)
            {
                return container;
            }
        }
    }
}