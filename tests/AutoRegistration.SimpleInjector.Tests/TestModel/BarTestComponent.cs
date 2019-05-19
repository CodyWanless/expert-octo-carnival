using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class BarTestComponent : ITestComponent
    {
        public void Foo()
        {
            Console.WriteLine("Bar");
        }
    }
}