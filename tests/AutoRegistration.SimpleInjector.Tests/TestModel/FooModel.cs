using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class FooModel : IModel
    {
        public Guid Key => Guid.NewGuid();
    }
}