using BaseAPI.Database.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseAPI.Database.Configurations.Identity;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).UseIdentityAlwaysColumn();

        builder.Property(x => x.CreatedOnDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(u => u.AuthProvider)
            .WithMany()
            .HasForeignKey(u => u.AuthProviderId);
    }
}
