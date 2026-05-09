using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Enrollment
{
    public long EnrollmentId { get; set; }

    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string? ClassId { get; set; }

    public int StatusId { get; set; }

    public string? ApprovedByTeacherId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? Note { get; set; }

    public virtual Teacher? ApprovedByTeacher { get; set; }

    public virtual CenterClass? Class { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual EnrollmentStatus Status { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
