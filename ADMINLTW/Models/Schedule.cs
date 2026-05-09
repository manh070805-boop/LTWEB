using System;
using System.Collections.Generic;

namespace TrungTamLapTrinh.Web.Models;

public partial class Schedule
{
    public long ScheduleId { get; set; }

    public string ClassId { get; set; } = null!;

    public byte DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? RoomName { get; set; }

    public virtual CenterClass Class { get; set; } = null!;
}
