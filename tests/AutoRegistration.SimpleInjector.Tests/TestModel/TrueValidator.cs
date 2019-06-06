namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class TrueValidator<T> : IValidator<T>
    {
        public bool Validate(T item)
        {
            return true;
        }
    }
}