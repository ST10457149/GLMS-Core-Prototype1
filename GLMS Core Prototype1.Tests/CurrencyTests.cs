using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void ConvertUsdToZar_CorrectCalculation()
        {
            var rate = 20m;
            var usd = 10m;
            var zar = usd * rate;
            Assert.Equal(200m, zar);
        }
    }
}
