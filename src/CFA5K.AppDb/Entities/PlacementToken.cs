// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA5K.AppDb.Entities;

public class PlacementToken
{
    public virtual Guid Id { get; set; }

    public virtual DateTime CreatedDate { get; set; }

    public virtual Occasion Occasion { get; set; } = default!;
    public virtual Guid OccasionId { get; set; }

    public virtual int Position { get; set; }

    public virtual DateTime? FinishTime { get; set; }

    internal class ModelConfig : IEntityTypeConfiguration<PlacementToken>
    {
        public void Configure(EntityTypeBuilder<PlacementToken> builder)
        {
            builder.Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("current_timestamp");

            builder.HasIndex(x => new
            {
                x.OccasionId,
                x.Position,
            }).IsUnique();
        }
    }
}
