﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using AutoRegistration.Abstract;
using System.Collections;
using System.Collections.Generic;
using SimpleInjector.Lifestyles;
using Microsoft.Extensions.DependencyInjection;
using AutoRegistration.SimpleInjector.Tests.TestModel;

namespace AutoRegistration.SimpleInjector.Tests
{
    [TestClass]
    public class SimpleInjectorContainerAdapterTest
    {
        private SimpleInjectorContainerAdapter containerAdapter;

        [TestInitialize]
        public void Init()
        {
            var emptyContainer = new Container();
            containerAdapter = new SimpleInjectorContainerAdapter(emptyContainer);
        }

        [TestMethod]
        public void Register_SingleInterfaceSingleImpl()
        {
            containerAdapter.Register(typeof(ITestComponent), typeof(TestComponent), Types.Scope.Transient);

            VerifyContainer(containerAdapter, new[] { (typeof(ITestComponent), typeof(TestComponent)) });
        }

        [TestMethod]
        public void Register_DecoratorTest()
        {
            containerAdapter.RegisterDecorator(typeof(ITestComponent),
                typeof(TestComponentDecorator), Types.Scope.Transient);
            containerAdapter.Register(typeof(ITestComponent), typeof(TestComponent), Types.Scope.Transient);

            VerifyContainer(containerAdapter, new[] { (typeof(ITestComponent), typeof(TestComponentDecorator)) });
        }

        [TestMethod]
        public void Register_CollectionRegistrationTest()
        {
            containerAdapter.RegisterAll(typeof(ITestComponent), new[]
            {
                (typeof(BarTestComponent), Types.Scope.Transient),
                (typeof(FooTestComponent), Types.Scope.Transient),
                (typeof(TestComponent), Types.Scope.Transient),
            });

            var runtimeContainer = VerifyContainer(containerAdapter);
            var componentCollection = runtimeContainer.GetService(typeof(IEnumerable<ITestComponent>));

            Assert.IsNotNull(componentCollection);
        }

        [TestMethod]
        public void Register_CompositeTest()
        {
            containerAdapter.RegisterAll(typeof(ITestComponent), new[]
            {
                (typeof(BarTestComponent), Types.Scope.Transient),
                (typeof(FooTestComponent), Types.Scope.Transient),
                (typeof(TestComponent), Types.Scope.Transient),
            });
            containerAdapter.Register(typeof(ITestComponent), typeof(CompositeTestComponent), Types.Scope.Transient);

            VerifyContainer(containerAdapter, new[] { (typeof(ITestComponent), typeof(CompositeTestComponent)) });
        }

        [TestMethod]
        public void Register_DecoratedCompositeTest()
        {
            containerAdapter.RegisterAll(typeof(ITestComponent), new[]
            {
                (typeof(BarTestComponent), Types.Scope.Transient),
                (typeof(FooTestComponent), Types.Scope.Transient),
                (typeof(TestComponent), Types.Scope.Transient),
            });
            containerAdapter.Register(typeof(ITestComponent), typeof(CompositeTestComponent), Types.Scope.Transient);
            containerAdapter.RegisterDecorator(typeof(ITestComponent), typeof(TestComponentDecorator), Types.Scope.Transient);

            VerifyContainer(containerAdapter, new[] { (typeof(ITestComponent), typeof(TestComponentDecorator)) });
        }

        private static IServiceProvider VerifyContainer(IRegisterTimeContainer container,
            IEnumerable<(Type serviceType, Type runtimeType)> typesToVerify = null)
        {
            container.Verify();

            var runtimeContainer = container.ToRuntimeContainer();

            if (typesToVerify != null)
            {
                foreach (var (serviceType, runtimeType) in typesToVerify)
                {
                    var runtimeService = runtimeContainer.GetService(serviceType);

                    Assert.IsInstanceOfType(runtimeService, runtimeType);
                }
            }

            return runtimeContainer;
        }
    }
}
