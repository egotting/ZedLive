using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZedLive.Domain.User;

namespace ZedLive.Infrastructure.Data.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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
        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(266);
            email.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsOne(x => x.Password, pass =>
        {
            pass.Property(p => p.Value)
                .HasColumnName("password")
                .HasColumnType("varchar")
                .IsRequired();
        });

        builder.Property(x => x.Salt)
            .HasColumnName("salt")
            .IsRequired();

        builder.Property(x => x.StreamKey)
            .HasColumnName("stream_key");
        builder.HasIndex(x => x.StreamKey)
            .IsUnique();

        builder.OwnsMany<StatusUser>("Status", b =>
        {
            b.WithOwner().HasForeignKey("Id");
            b.ToTable("tb_status_user", "dbo");
            b.Property<long>("Id");
            b.Property<string>("Value").HasColumnType("varchar");
            b.HasKey("Id", "Value");
        });
    }
}