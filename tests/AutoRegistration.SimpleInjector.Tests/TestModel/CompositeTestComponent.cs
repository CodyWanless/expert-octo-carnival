using System.Collections.Generic;
using System.Linq;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class CompositeTestComponent : ITestComponent
    {
        private readonly IReadOnlyCollection<ITestComponent> components;

        public CompositeTestComponent(IEnumerable<ITestComponent> components)
        {
            this.components = components.ToArray();
        }

        public void Foo()
        {
            foreach (var comp in components)
            {
                comp.Foo();
            }
        }
    }
}