using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Observer_Pattern
{
    public class AuditService : IObserver
    {
        public void Update(Contract contract)
        {
            Console.WriteLine($"Audit Log: Contract {contract.ContractId} updated at {DateTime.Now}");
        }
    }
}