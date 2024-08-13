using BG.LocalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.LocalApi.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Genre)
                .IsRequired();

            builder.Property(b => b.PublicationYear)
                .IsRequired();

            // since it was not requested...
            //// Relationships
            //builder.HasOne(b => b.Author)
            //    .WithMany(a => a.Books)
            //    .HasForeignKey(b => b.AuthorId)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
