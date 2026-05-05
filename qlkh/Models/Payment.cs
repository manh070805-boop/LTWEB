using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public long PaymentId { get; set; }

        public long EnrollmentId { get; set; }

        public decimal Amount { get; set; }

        public int MethodId { get; set; }

        public int StatusId { get; set; }

        public string? TransactionNo { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        [ForeignKey("EnrollmentId")]
        public Enrollment? Enrollment { get; set; }

        [ForeignKey("MethodId")]
        public PaymentMethod? Method { get; set; }

        [ForeignKey("StatusId")]
        public PaymentStatus? Status { get; set; }
    }
}