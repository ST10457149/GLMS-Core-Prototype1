using GLMS_Core_Prototype.Models;

namespace GLMS_Core_Prototype.Patterns.Strategy_Pattern
{
    
        public interface IValidationStrategy
        {
            bool Validate(Contract contract);
        }
    }
