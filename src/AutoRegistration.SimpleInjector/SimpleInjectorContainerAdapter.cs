using System;
using System.Linq;
using AutoRegistration.Abstract;
using SimpleInjector;

namespace AutoRegistration.SimpleInjector
{
    public class SimpleInjectorContainerAdapter : IRegisterTimeContainer
    {
        private readonly Container container;

        public SimpleInjectorContainerAdapter(Container container)
        {
            this.container = container;
        }

        public IRegisterTimeContainer Register(Type service, Type implementation, Types.Scope scope)
        {
            container.Register(service, implementation, ConvertLifetime(scope));

            return this;
        }

        public IRegisterTimeContainer RegisterAll(Type service, System.Collections.Generic.IEnumerable<(Type type, Types.Scope scope)> implementations)
        {
            if (service.IsOpenGeneric())
            {
                foreach (var impl in implementations)
                {
                    container.Collection.Append(service, impl.type.ToTypeKey());
                }
            }
            else
            {
                container.Collection.Register(service,
                    implementations.Select(impl => ConvertLifetime(impl.scope).CreateRegistration(impl.type, container)));
            }

            return this;
        }

        public IRegisterTimeContainer RegisterDecorator(Type service, Type implementation, Types.Scope scope)
        {
            container.RegisterDecorator(service, implementation, ConvertLifetime(scope));

            return this;
        }

        public void Verify()
        {
            container.Verify();
        }

        public IServiceProvider ToRuntimeContainer()
        {
            return container;
        }

        private Lifestyle ConvertLifetime(Types.Scope scope)
        {
            switch (scope)
            {
                case Types.Scope.Singleton:
                    return Lifestyle.Singleton;
                case Types.Scope.Scoped:
                    return Lifestyle.Scoped;
                case Types.Scope.Transient:
                    return Lifestyle.Transient;
                default:
                    throw new ArgumentOutOfRangeException($"{scope} is not a valid {nameof(scope)}");
            }
        }
    }
}
