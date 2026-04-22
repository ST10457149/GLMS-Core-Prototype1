

using System.ComponentModel.DataAnnotations;

namespace GLMS_Core_Prototype.Models
{
    public enum ContractStatus { Draft, Active, Expired, OnHold }

    public class Contract
    {
        public int ContractId { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public ContractStatus Status { get; set; }
        [Required]
        [StringLength(100)]
        public string ServiceLevel { get; set; }
        [StringLength(100)]
        public string? Region { get; set; }
        public string? SignedAgreementPath { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}