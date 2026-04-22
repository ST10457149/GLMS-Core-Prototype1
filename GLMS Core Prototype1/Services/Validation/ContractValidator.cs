using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Services.Validation
{
    public class ContractValidator : IModelValidator<Contract>
    {
        public IEnumerable<string> Validate(Contract model)
        {
            if (model.ClientId <= 0)
                yield return "Client selection is required.";
            if (string.IsNullOrWhiteSpace(model.ServiceLevel))
                yield return "Service level is required.";
            if (model.StartDate == default)
                yield return "Start date is required.";
            if (model.EndDate == default)
                yield return "End date is required.";
            if (model.EndDate < model.StartDate)
                yield return "End date must be on or after the start date.";
        }
    }
}
