using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnType("integer");

        builder
            .Property(x => x.FirstName)
            .HasColumnType("varchar");

        builder
            .Property(x => x.LastName)
            .HasColumnType("varchar");
    }
}