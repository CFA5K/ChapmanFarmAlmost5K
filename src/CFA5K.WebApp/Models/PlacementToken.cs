// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Postgrest.Attributes;
using Postgrest.Models;

namespace CFA5K.WebApp.Models;

[Table("PlacementTokens")]
public class PlacementToken : BaseModel
{
    [Column(nameof(Id))]
    public Guid Id { get; set; }
    [Column(nameof(CreatedDate))]
    public DateTime CreatedDate { get; set; }
    public Guid OccasionId { get; set; }
    [Column(nameof(Position))]
    public int Position { get; set; }
    [Column(nameof(FinishTime))]
    public DateTime? FinishTime { get; set; }
}
