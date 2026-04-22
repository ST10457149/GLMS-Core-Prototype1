using GLMS_Core_Prototype.Models;   
    
namespace GLMS_Core_Prototype.Patterns.Observer_Pattern
{
    public class NotificationService : IObserver
    {
        public void Update(Contract contract)
        {
            Console.WriteLine($"Notification: Contract {contract.ContractId} changed to {contract.Status}");
        }
    }
}
