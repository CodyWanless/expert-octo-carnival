namespace AutoRegistration.SimpleInjector.Tests.TestModel
{
    public class AStringValidator : IValidator<string>
    {
        public bool Validate(string item)
        {
            return true;
        }
    }
}