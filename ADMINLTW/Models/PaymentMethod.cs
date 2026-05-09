using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class PaymentMethod
{
    public int MethodId { get; set; }

    public string MethodCode { get; set; } = null!;

    public string MethodName { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
