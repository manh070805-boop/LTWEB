using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class PaymentStatus
{
    public int StatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
