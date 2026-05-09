using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class CenterClass
{
    public string ClassId { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string? RoomName { get; set; }

    public string CourseId { get; set; } = null!;

    public string TeacherId { get; set; } = null!;

    public int StatusId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? MaxStudents { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ClassStatus Status { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
