using System;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class TestComponentDecorator : ITestComponent
    {
        private readonly ITestComponent decoratedComponent;

        public TestComponentDecorator(ITestComponent decoratedComponent)
        {
            this.decoratedComponent = decoratedComponent;
        }

        public void Foo()
        {
            Console.WriteLine("Decorated");
            this.decoratedComponent.Foo();
        }
    }
}
