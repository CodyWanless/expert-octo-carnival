using System;
using System.Collections.Generic;
using System.Reflection;
using AutoRegistration.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRegistration.Core.WebApi
{
	public sealed class RegistrationEngine : IRegistrationEngine
	{
		private readonly IRegistrationStrategy registrationStrategy;

		public RegistrationEngine(IRegistrationStrategy registrationStrategy)
		{
			this.registrationStrategy = registrationStrategy;
		}

		public IServiceProvider ConfigureContainer(IApplicationBuilder app, IReadOnlyCollection<Assembly> assemblies)
		{
			return ConfigureContainer(app, assemblies, new IRegistrationConvention[0]);
		}

		public IServiceProvider ConfigureContainer(IApplicationBuilder app, IReadOnlyCollection<Assembly> assemblies, IReadOnlyCollection<IRegistrationConvention> customRegistrations)
		{
			var container = registrationStrategy.ConfigureContainer(app, assemblies, customRegistrations);

			container.Verify();
			return container.ToRuntimeContainer();
		}

		public void IntegrateContainer(IServiceCollection services)
		{
			registrationStrategy.IntegrateContainer(services);
		}
	}
}
