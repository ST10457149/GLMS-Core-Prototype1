using System.ComponentModel.DataAnnotations;

namespace GLMS_Core_Prototype.Models
{
    public enum RequestStatus { Pending, Approved, Rejected }

    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal CostUSD { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostZAR { get; set; }
        public RequestStatus Status { get; set; }
    }
}
