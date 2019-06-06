namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class StringEmptyValidator : IValidator<string>
    {
        public bool Validate(string item)
        {
            return !string.IsNullOrEmpty(item);
        }
    }
}