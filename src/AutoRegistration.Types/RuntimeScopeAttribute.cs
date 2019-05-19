using System;

namespace AutoRegistration.Types
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class RuntimeScopeAttribute : Attribute
	{
		public RuntimeScopeAttribute(Scope scope)
		{
			Scope = scope;
		}

		public Scope Scope { get; }
	}
}
