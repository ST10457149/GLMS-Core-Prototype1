using GLMS_Core_Prototype.Models;
using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class ClientTests
    {
        [Fact]
        public void CreateClient_SetsPropertiesCorrectly()
        {
            var client = new Client { Name = "Test", ContactDetails = "123", Region = "ZA" };
            Assert.Equal("Test", client.Name);
            Assert.Equal("123", client.ContactDetails);
            Assert.Equal("ZA", client.Region);
        }
    }
}
