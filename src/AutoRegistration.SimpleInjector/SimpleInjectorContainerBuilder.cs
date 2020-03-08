using AutoRegistration.Abstract;
using SimpleInjector;

namespace AutoRegistration.SimpleInjector
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