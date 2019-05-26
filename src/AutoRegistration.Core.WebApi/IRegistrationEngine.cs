using System;
using System.Collections.Generic;
using System.Reflection;
using AutoRegistration.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRegistration.Core.WebApi
{
    public interface IRegistrationEngine
    {
        void IntegrateContainer(IServiceCollection services);

        IServiceProvider ConfigureContainer(IApplicationBuilder app,
            IReadOnlyCollection<Assembly> assemblies);

        IServiceProvider ConfigureContainer(IApplicationBuilder app,
            IReadOnlyCollection<Assembly> assemblies,
            IReadOnlyCollection<IRegistrationConvention> customRegistrations);
    }
}