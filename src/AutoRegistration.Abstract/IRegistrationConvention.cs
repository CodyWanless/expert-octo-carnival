using System;
using System.Collections.Generic;

namespace AutoRegistration.Abstract
{
	public interface IRegistrationConvention
	{
		IRegisterTimeContainer Register(IReadOnlyCollection<Type> types,
			IRegisterTimeContainer container);
	}
}
