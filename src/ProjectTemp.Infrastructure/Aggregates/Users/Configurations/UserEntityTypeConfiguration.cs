using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTemp.Domain.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.ValueObjects;

namespace ProjectTemp.Infrastructure.Aggregates.Users.Configurations
{
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(WriteDbContext.Users));

            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.Id)
                .HasConversion(i => i.Value, i => UserId.Create(i));

            builder.Property(i => i.Description)
                .HasConversion(i => i.Value, i => Description.Create(i));
            
            builder.Property(i => i.Username)
                .IsRequired()
                .HasConversion(i => i.Value, i => UserUsername.Create(i));

            builder.Property(i => i.Password)
                .IsRequired()
                .HasConversion(i => i.Value, i => UserPassword.Create(i));

            base.Configure(builder);
        }
    }
}