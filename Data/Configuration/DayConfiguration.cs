using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

internal class DayConfiguration : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.DayOfSchedule)
            .HasColumnType("integer");
    }
}