using System.Collections.Generic;
using System.Reflection;

namespace AutoRegistration.Abstract
{
    public interface IContainerBuilder
    {
        IRegisterTimeContainer BuildContainer(IReadOnlyCollection<Assembly> assemblies,
          IReadOnlyCollection<IRegistrationConvention> customRegistrationConventions);
    }
}