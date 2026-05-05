using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key]
        public int MethodId { get; set; }

        public string? MethodCode { get; set; } 
        public string? MethodName { get; set; }
    }
}