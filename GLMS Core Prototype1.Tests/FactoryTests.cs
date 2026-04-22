using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Patterns.Factory_Pattern;
using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class FactoryTests
    {
        [Fact]
        public void StandardRequestHandler_ApprovesRequest()
        {
            var request = new ServiceRequest();
            var handler = new StandardRequestHandler();
            handler.Handle(request);
            Assert.Equal(RequestStatus.Approved, request.Status);
        }

        [Fact]
        public void UrgentRequestHandler_SetsPending()
        {
            var request = new ServiceRequest();
            var handler = new UrgentRequestHandler();
            handler.Handle(request);
            Assert.Equal(RequestStatus.Pending, request.Status);
        }
    }
}
