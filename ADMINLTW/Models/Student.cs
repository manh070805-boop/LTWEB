using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public long AccountId { get; set; }

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly? Birthday { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
