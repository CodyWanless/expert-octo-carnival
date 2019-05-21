using System.Threading.Tasks;

namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public interface IRepository<T>
        where T : IModel
    {
        Task Save(T item);
    }
}