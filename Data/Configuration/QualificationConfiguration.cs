using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

internal class QualificationConfiguration : IEntityTypeConfiguration<Qualification>
{
    public void Configure(EntityTypeBuilder<Qualification> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnType("integer");

        builder
            .Property(x => x.Name)
            .HasColumnType("varchar");

        builder
            .HasMany<Machine>(x => x.Machines)
            .WithOne(s => s.RequiredQualification)
            .HasForeignKey(z=>z.Id);

        builder
            .HasMany<Person>(q => q.People)
            .WithMany(p => p.Qualifications);
    }
}