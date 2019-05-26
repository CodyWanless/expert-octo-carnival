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
            var autoRegistration = new TypePatternRegistrationConvention();

            var rawTypePairs = assemblies
                .SelectMany(assembly =>
                    assembly.GetExportedTypes())
                    .Where(type => !type.IsInterface)
                    .Where(type => !type.IsAbstract)
                    .Where(type => type.IsPublic)
                    .Where(type => !type.IsNested)
                    .SelectMany(type => type.GetInterfaces().Select(i => new { i, type }));

            var typeDictionary = new Dictionary<Type, IList<Type>>();
            foreach (var typePair in rawTypePairs)
            {
                if (!typeDictionary.ContainsKey(typePair.i))
                {
                    typeDictionary.Add(typePair.i, new List<Type>());
                }

                typeDictionary[typePair.i].Add(typePair.type);

            }

            // Go through custom registrations, removing types that match
            foreach (var customConvention in customRegistrations)
            {
                // TODO: cleanup this n^3 logic
                var typesToRegister = new List<Type>();
                foreach (var interfaceToRegister in customConvention.InterfacesToRegister)
                {
                    if (typeDictionary.TryGetValue(interfaceToRegister, out var types))
                    {
                        typesToRegister.AddRange(types);
                        typesToRegister.Remove(interfaceToRegister);
                    }
                }

                customConvention.Register(typesToRegister, containerAdapter);
            }

            // auto register left overs 
            autoRegistration.Register(typeDictionary.Values.SelectMany(t => t).ToArray(), containerAdapter);

            return containerAdapter;
        }
    }
}
