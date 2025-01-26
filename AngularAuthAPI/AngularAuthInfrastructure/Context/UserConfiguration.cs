using AngularAuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id); 
        builder.Property(u => u.UserName).IsRequired() .HasMaxLength(50);
        builder.Property(u => u.FirstName).HasMaxLength(50);
        builder.Property(u => u.LastName).HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
        builder.Property(u => u.ResetPasswordCode).HasMaxLength(500);
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}