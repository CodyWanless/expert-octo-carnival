using System;
using System.Collections.Generic;
using AutoRegistration.Types;

namespace AutoRegistration.Abstract
{
	public interface IRegisterTimeContainer
	{
		IRegisterTimeContainer Register(Type service, Type implementation, Scope scope);
		IRegisterTimeContainer RegisterAll(Type service, IEnumerable<(Type type, Scope scope)> implementations);
		IRegisterTimeContainer RegisterDecorator(Type service, Type implementation, Scope scope);

		void Verify();
		IServiceProvider ToRuntimeContainer();
	}
}
