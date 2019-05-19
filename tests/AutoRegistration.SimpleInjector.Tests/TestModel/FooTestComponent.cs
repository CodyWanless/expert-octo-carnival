using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class FooTestComponent : ITestComponent
    {
        public void Foo()
        {
            Console.WriteLine("The actual Foo component");
        }
    }
}