using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class TestComponent : ITestComponent
    {
        public void Foo()
        {
            Console.WriteLine("Foo");
        }
    }
}
