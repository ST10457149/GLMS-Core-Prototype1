using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Factory_Pattern
{
    public interface IRequestHandler
    {
        void Handle(ServiceRequest request);
    }
}