using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Factory_Pattern
{
    public class StandardRequestHandler : IRequestHandler
    {
        public void Handle(ServiceRequest request)
        {
            request.Status = RequestStatus.Approved;
        }
    }
}