using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoRegistration.Abstract
{
    public abstract class ContainerBuilder : IContainerBuilder
    {
        public IRegisterTimeContainer BuildContainer(IReadOnlyCollection<Assembly> assemblies,
            IReadOnlyCollection<IRegistrationConvention> customRegistrationConventions)
        {
            var container = CreateContainer();
            var autoRegistration = new TypePatternRegistrationConvention();

            var rawTypePairs = assemblies
                .SelectMany(assembly =>
                    assembly.GetExportedTypes())
                    .Where(type => !type.IsInterface)
                    .Where(type => !type.IsAbstract)
                    .Where(type => type.IsPublic)
                    .Where(type => !type.IsNested)
                    .SelectMany(type => type.GetInterfaces().Select(i => new { key = i.ToTypeKey(), type }));

            var typeDictionary = new Dictionary<Type, IList<Type>>();
            foreach (var typePair in rawTypePairs)
            {
                if (!typeDictionary.ContainsKey(typePair.key))
                {
                    typeDictionary.Add(typePair.key, new List<Type>());
                }

                typeDictionary[typePair.key].Add(typePair.type);
            }

            // Go through custom registrations, removing types that match
            foreach (var customConvention in customRegistrationConventions)
            {
                var typesToRegister = new List<Type>();
                foreach (var interfaceToRegister in customConvention.InterfacesToRegister
                    .Select(t => t.ToTypeKey()))
                {
                    if (typeDictionary.TryGetValue(interfaceToRegister, out var types))
                    {
                        typesToRegister.AddRange(types);
                        typeDictionary.Remove(interfaceToRegister);
                    }
                }

                customConvention.Register(typesToRegister, container);
            }

            // auto register left overs 
            autoRegistration.Register(typeDictionary.Values.SelectMany(t => t).ToArray(), container);

            return container;
        }

        public IRegisterTimeContainer BuildContainer(IReadOnlyCollection<Assembly> assemblies)
        {
            return BuildContainer(assemblies, new IRegistrationConvention[0]);
        }

        protected abstract IRegisterTimeContainer CreateContainer();
    }
}