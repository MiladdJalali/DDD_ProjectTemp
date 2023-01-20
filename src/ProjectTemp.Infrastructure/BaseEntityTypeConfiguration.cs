using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTemp.Domain;

namespace ProjectTemp.Infrastructure;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<Guid>("CreatorId");
        builder.Property<Guid?>("UpdaterId");
        builder.Property<DateTimeOffset>("Created");
        builder.Property<DateTimeOffset?>("Updated");
    }
}