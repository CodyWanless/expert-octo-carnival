using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoRegistration.Abstract;
using AutoRegistration.SimpleInjector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace AutoRegistration.Core.WebApi.SimpleInjector
{
    public class SimpleInjectorRegistrationStrategy : IRegistrationStrategy
    {
        private readonly Container container;

        public SimpleInjectorRegistrationStrategy()
        {
            this.container = new Container();
        }

        public void IntegrateContainer(IServiceCollection services)
        {
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    .AddControllerActivation()
                    .AddViewComponentActivation()
                    .AddPageModelActivation()
                    .AddTagHelperActivation();
            });
        }

        public IRegisterTimeContainer ConfigureContainer(IApplicationBuilder app, IReadOnlyCollection<Assembly> assemblies, IReadOnlyCollection<IRegistrationConvention> customRegistrations)
        {
            var containerAdapter = new SimpleInjectorContainerAdapter(container);
            var containerBuilder = ContainerBuilderFactory.CreateContainerBuilder(containerAdapter);

            return containerBuilder.BuildContainer(assemblies, customRegistrations);
        }
    }
}
