using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Teacher
{
    public string TeacherId { get; set; } = null!;

    public long AccountId { get; set; }

    public string Phone { get; set; } = null!;

    public string? Specialization { get; set; }

    public string? Bio { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CenterClass> CenterClasses { get; set; } = new List<CenterClass>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
