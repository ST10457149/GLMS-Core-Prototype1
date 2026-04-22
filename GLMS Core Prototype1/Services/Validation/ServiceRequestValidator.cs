using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Patterns.Strategy_Pattern;

namespace GLMS_Core_Prototype.Services.Validation
{
    public class ServiceRequestValidator
    {
        private readonly IValidationStrategy _contractStrategy;
        private readonly IValidationStrategy _regionalStrategy;

        public ServiceRequestValidator()
        {
            _contractStrategy = new ContractValidationStrategy();
            _regionalStrategy = new RegionalValidationStrategy();
        }

        public IEnumerable<string> Validate(ServiceRequest request, Contract? contract)
        {
            if (contract == null)
            {
                yield return "Contract is required.";
                yield break;
            }

            if (!_contractStrategy.Validate(contract))
                yield return "Contract is not valid for requests.";

            if (!_regionalStrategy.Validate(contract))
                yield return "Contract region is restricted.";

            if (string.IsNullOrWhiteSpace(request.Description))
                yield return "Description is required.";

            if (request.CostUSD <= 0)
                yield return "Cost must be greater than zero.";
        }
    }
}
