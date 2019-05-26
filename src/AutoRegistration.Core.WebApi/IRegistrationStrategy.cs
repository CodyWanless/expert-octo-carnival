using System.Collections.Generic;
using System.Reflection;
using AutoRegistration.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRegistration.Core.WebApi
{
    public interface IRegistrationStrategy
    {
        void IntegrateContainer(IServiceCollection services);

        IRegisterTimeContainer ConfigureContainer(IApplicationBuilder app,
            IReadOnlyCollection<Assembly> assemblies,
            IReadOnlyCollection<IRegistrationConvention> customRegistrations);
    }
}
