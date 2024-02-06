// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Postgrest.Attributes;
using Postgrest.Models;

namespace CFA5K.WebApp.Models;

[Table("Occasions")]
public class Occasion : BaseModel
{
    [Column(nameof(Id))]
    public Guid Id { get; set; }
    [Column(nameof(CreatedDate))]
    public DateTime CreatedDate { get; set; }
    [Column(nameof(Name))]
    public string Name { get; set; } = default!;
    [Column(nameof(ScheduledDate))]
    public DateTime? ScheduledDate { get; set; }
    [Column(nameof(StartTime))]
    public DateTime? StartTime { get; set; }
}
