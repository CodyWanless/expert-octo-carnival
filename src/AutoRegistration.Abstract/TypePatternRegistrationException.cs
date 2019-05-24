using System;

namespace AutoRegistration.Abstract
{
    public class TypePatternRegistrationException : Exception
    {
        public TypePatternRegistrationException(string message)
            : base(message) { }
    }
}