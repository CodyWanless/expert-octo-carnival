using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class BarModel : IModel
    {
        public Guid Key => Guid.NewGuid();
    }
}