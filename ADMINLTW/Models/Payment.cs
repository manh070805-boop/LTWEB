using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Payment
{
    public long PaymentId { get; set; }

    public long EnrollmentId { get; set; }

    public decimal Amount { get; set; }

    public int MethodId { get; set; }

    public int StatusId { get; set; }

    public string? TransactionNo { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;

    public virtual PaymentMethod Method { get; set; } = null!;

    public virtual PaymentStatus Status { get; set; } = null!;
}
