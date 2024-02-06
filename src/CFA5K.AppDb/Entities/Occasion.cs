// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA5K.AppDb.Entities;

public class Occasion
{
    public virtual Guid Id { get; set; }

    public virtual DateTime CreatedDate { get; set; }

    public virtual string Name { get; set; } = default!;

    public virtual DateTime? ScheduledDate { get; set; }

    public virtual DateTime? StartTime { get; set; }

    internal class ModelConfig : IEntityTypeConfiguration<Occasion>
    {
        public void Configure(EntityTypeBuilder<Occasion> builder)
        {
            builder.Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("current_timestamp");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
