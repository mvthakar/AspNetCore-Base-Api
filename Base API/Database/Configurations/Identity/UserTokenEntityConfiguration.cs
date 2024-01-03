using BaseAPI.Database.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseAPI.Database.Configurations.Identity;

public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.Property(x => x.Id).UseIdentityAlwaysColumn();
        builder.Property(x => x.IssuedOnDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(ut => ut.User).WithMany().HasForeignKey(ut => ut.UserId);
        builder.HasOne(ut => ut.AuthProvider).WithMany().HasForeignKey(ut => ut.AuthProviderId);
        builder.HasOne(ut => ut.TokenType).WithMany().HasForeignKey(ut => ut.TokenTypeId);
    }
}
