using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZedLive.Domain.User;

namespace ZedLive.Infrastructure.Data.Mapping;

public class StatusUserMapping : IEntityTypeConfiguration<StatusUser>
{
    public void Configure(EntityTypeBuilder<StatusUser> builder)
    {
        builder.ToTable("tb_status_user", "dbo");

        builder.Property<byte>("id");
        builder.Property<string>("value")
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.HasKey("id");

        builder.HasData(
            new { id = (byte)1, value = "Online" },
            new { id = (byte)2, value = "Offline" },
            new { id = (byte)3, value = "Inactive" }
        );
    }
}