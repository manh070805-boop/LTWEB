using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class EnrollmentStatus
{
    public int StatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
