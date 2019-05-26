namespace AutoRegistration.Abstract
{
    public static class ContainerBuilderFactory
    {
        public static IContainerBuilder CreateContainerBuilder(IRegisterTimeContainer container)
        {
            return new ContainerBuilder(container);
        }
    }
}