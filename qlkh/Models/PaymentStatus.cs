using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("PaymentStatus")]
    public class PaymentStatus
    {
         [Key]
        public int StatusId { get; set; }

        public string? StatusCode { get; set; }

        public string? StatusName { get; set; }
    }
}