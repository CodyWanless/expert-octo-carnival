using AutoRegistration.Abstract;
using AutoRegistration.SimpleInjector;
using SimpleInjector;

namespace AutoRegistration.Core.WebApi.SimpleInjector
{
    public sealed class SimpleInjectorContainerBuilder : ContainerBuilder
    {
        private readonly Container container;

        public SimpleInjectorContainerBuilder(Container container)
        {
            this.container = container;
        }

        protected override IRegisterTimeContainer CreateContainer()
        {
            return new SimpleInjectorContainerAdapter(container);
        }
    }
}