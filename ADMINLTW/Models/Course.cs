using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public int? DurationMonths { get; set; }

    public string? Roadmap { get; set; }

    public int LevelId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CenterClass> CenterClasses { get; set; } = new List<CenterClass>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual CourseLevel Level { get; set; } = null!;
}
