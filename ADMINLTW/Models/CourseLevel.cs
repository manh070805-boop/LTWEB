using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class CourseLevel
{
    public int LevelId { get; set; }

    public string LevelCode { get; set; } = null!;

    public string LevelName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
