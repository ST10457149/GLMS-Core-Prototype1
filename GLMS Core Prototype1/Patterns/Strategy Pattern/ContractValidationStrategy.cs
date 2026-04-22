using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Strategy_Pattern
{
    public class ContractValidationStrategy : IValidationStrategy
    {
        public bool Validate(Contract contract)
        {
            return contract.Status == ContractStatus.Active;
        }
    }
}