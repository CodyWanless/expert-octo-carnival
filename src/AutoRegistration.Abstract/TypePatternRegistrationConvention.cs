using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoRegistration.Abstract
{
    internal sealed class TypePatternRegistrationConvention : IRegistrationConvention
    {
        // This needs to be the catch all
        public IEnumerable<Type> InterfacesToRegister { get; } = Enumerable.Empty<Type>();

        public IRegisterTimeContainer Register(IReadOnlyCollection<Type> types, IRegisterTimeContainer container)
        {
            var implementationTypes = types
                .Where(type => type.GetInterfaces().Length == 1)
                .Where(type => type.GetConstructors().Length == 1)
                .ToArray();

            var byInterface = implementationTypes
                .SelectMany(type =>
                {
                    return type.GetInterfaces().Select(i => new[]
                    {
                        i.ToTypeKey(),
                        type
                    });
                })
                .GroupBy(pair => pair[0], pair => pair[1]);

            foreach (var registrationGroup in byInterface)
            {
                var inter = registrationGroup.Key;
                var implementations = registrationGroup.ToList();

                var composites = implementations.Where(type => type.IsComposite()).ToArray();
                var decorators = implementations.Where(type => type.IsDecorator()).ToArray();

                var normals = implementations.Except(composites).Except(decorators).ToArray();

                if (composites.Length > 1)
                {
                    throw new TypePatternRegistrationException("You cannot register more than one composite.");
                }
                if (!composites.Any() && !normals.Any())
                {
                    throw new TypePatternRegistrationException("You cannot register only decorators.");
                }

                if (composites.Any())
                {
                    var composite = composites.Single();
                    container.Register(inter, composite, GetScope(composite));
                }

                if (normals.Length == 1)
                {
                    var normal = normals.Single();
                    container.Register(inter, normal, GetScope(normal));
                }
                else if (normals.Any())
                {
                    container.RegisterAll(inter, normals.Select(type => (type, GetScope(type))));
                }

                foreach (var decorator in decorators)
                {
                    container.RegisterDecorator(inter, decorator, GetScope(decorator));
                }
            }

            return container;
        }

        private static Types.Scope GetScope(Type t)
        {
            var scopeAttribute = t.GetCustomAttribute<Types.RuntimeScopeAttribute>();
            if (scopeAttribute != null)
            {
                return scopeAttribute.Scope;
            }

            return Types.Scope.Transient;
        }
    }
}