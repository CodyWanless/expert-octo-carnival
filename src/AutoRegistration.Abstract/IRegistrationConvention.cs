using System;
using System.Collections.Generic;

namespace AutoRegistration.Abstract
{
    public interface IRegistrationConvention
    {
        IEnumerable<Type> InterfacesToRegister { get; }

        IRegisterTimeContainer Register(IReadOnlyCollection<Type> types,
            IRegisterTimeContainer container);
    }
}
