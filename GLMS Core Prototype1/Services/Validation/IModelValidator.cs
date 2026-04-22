namespace GLMS_Core_Prototype.Services.Validation
{
    public interface IModelValidator<T>
    {
        IEnumerable<string> Validate(T model);
    }
}
