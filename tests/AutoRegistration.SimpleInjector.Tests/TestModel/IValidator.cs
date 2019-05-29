namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public interface IValidator<T>
    {
        bool Validate(T item);
    }
}