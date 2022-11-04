using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

internal class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnType("integer");

        builder
            .Property(x => x.Name)
            .HasColumnType("varchar");

        builder.HasOne<Qualification>(x => x.RequiredQualification)
            .WithMany(z => z.Machines)
            .HasForeignKey(y => y.QualificationId);
    }
}