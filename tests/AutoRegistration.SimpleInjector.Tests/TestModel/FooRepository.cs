using System.Threading.Tasks;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class FooRepository : IRepository<FooModel>
    {
        public Task Save(FooModel item)
        {
            return Task.CompletedTask;
        }
    }
}