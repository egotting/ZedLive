using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZedLive.Domain.User;

namespace ZedLive.Infrastructure.Data.Mapper.User;

public class UserMapping : IEntityTypeConfiguration<Domain.User.User>
{
    public void Configure(EntityTypeBuilder<Domain.User.User> builder)
    {
        builder.ToTable("tb_usuarios", "dbo")
            .HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Login)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.HasIndex(x => x.Login)
            .IsUnique();
        builder.Property(x => x.Email)
            .HasMaxLength(266);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.Property(x => x.Password)
            .HasMaxLength(8)
            .IsRequired();

        builder.OwnsMany<StatusUser>("Status", b =>
        {
            b.WithOwner().HasForeignKey("Id");
            b.ToTable("StatusUser", "dbo");
            b.Property<string>("Id");
            b.Property<string>("Value").HasColumnType("StatusCode");
            b.HasKey("Id", "Value");
        });
    }
}