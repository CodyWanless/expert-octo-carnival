namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class NullValidator<T> : IValidator<T>
    {
        public bool Validate(T item)
        {
            return !Equals(item, default(T));
        }
    }
}