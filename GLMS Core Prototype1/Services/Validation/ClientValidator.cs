using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Services.Validation
{
    public class ClientValidator : IModelValidator<Client>
    {
        public IEnumerable<string> Validate(Client model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                yield return "Client name is required.";
            if (string.IsNullOrWhiteSpace(model.ContactDetails))
                yield return "Contact details are required.";
            if (string.IsNullOrWhiteSpace(model.Region))
                yield return "Region is required.";
        }
    }
}
