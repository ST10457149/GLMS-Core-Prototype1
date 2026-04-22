using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Patterns.Strategy_Pattern;
using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class StrategyTests
    {
        [Fact]
        public void ContractValidationStrategy_AllowsActiveContracts()
        {
            var contract = new Contract { Status = ContractStatus.Active };
            var strategy = new ContractValidationStrategy();
            Assert.True(strategy.Validate(contract));
        }

        [Fact]
        public void RegionalValidationStrategy_BlocksRestrictedRegion()
        {
            var contract = new Contract { Region = "Restricted" };
            var strategy = new RegionalValidationStrategy();
            Assert.False(strategy.Validate(contract));
        }
    }
}
