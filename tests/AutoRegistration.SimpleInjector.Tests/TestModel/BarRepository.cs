using System.Threading.Tasks;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class BarRepository : IRepository<BarModel>
    {
        public Task Save(BarModel item)
        {
            return Task.CompletedTask;
        }
    }
}