using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Observer_Pattern
{
    public interface IObserver
    {
        void Update(Contract contract);
    }
}
