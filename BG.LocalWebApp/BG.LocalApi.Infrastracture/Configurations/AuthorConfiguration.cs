using BG.LocalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.LocalApi.Infrastructure.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.OwnsOne(a => a.DateOfBirth, dob =>
            {
                dob.Property(d => d.Value)
                    .HasColumnName("DateOfBirth")
                    .IsRequired();
            });

            // since it was not requested ...
            //// Relationships
            //builder.HasMany(a => a.Books)
            //    .WithOne(b => b.Author)
            //    .HasForeignKey(b => b.AuthorId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Additional configurations can be added here as needed
        }
    }
}
