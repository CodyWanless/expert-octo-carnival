using System.Collections.Generic;
using System.Reflection;
using AutoRegistration.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace AutoRegistration.Core.WebApi.SimpleInjector
{
	public class SimpleInjectorRegistrationStrategy : IRegistrationStrategy
	{
		private readonly Container container;

		public SimpleInjectorRegistrationStrategy()
		{
			this.container = new Container();
			this.container.Options.ResolveUnregisteredConcreteTypes = false;
		}

		public void IntegrateContainer(IServiceCollection services)
		{
			services.AddSimpleInjector(container, options =>
			{
				// AddAspNetCore() wraps web requests in a Simple Injector scope.
				options.AddAspNetCore()
					// Ensure activation of a specific framework type to be created by
					// Simple Injector instead of the built-in configuration system.
					.AddControllerActivation()
					.AddViewComponentActivation();
			});
		}

		public IRegisterTimeContainer ConfigureContainer(IApplicationBuilder app, IReadOnlyCollection<Assembly> assemblies, IReadOnlyCollection<IRegistrationConvention> customRegistrations)
		{
			var containerBuilder = new SimpleInjectorContainerBuilder(container);

			return containerBuilder.BuildContainer(assemblies, customRegistrations);
		}
	}
}
