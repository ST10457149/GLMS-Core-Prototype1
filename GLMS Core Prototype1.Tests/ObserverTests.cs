using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Patterns.Observer_Pattern;
using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class ObserverTests
    {
        [Fact]
        public void NotificationService_LogsContractChange()
        {
            var contract = new Contract { ContractId = 1, Status = ContractStatus.Active };
            var observer = new NotificationService();
            observer.Update(contract);
        }

        [Fact]
        public void AuditService_LogsContractUpdate()
        {
            var contract = new Contract { ContractId = 2, Status = ContractStatus.Expired };
            var observer = new AuditService();
            observer.Update(contract);
        }
    }
}
