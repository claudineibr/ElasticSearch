using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElasticSearch.Domain.Classes;

namespace ElasticSearch.Repository.EntityConfiguration
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("TB_CATEGORY");
            builder.HasKey(c => c.CategoryCode).HasName("CategoryCode");
            builder.Property(c => c.CategoryCode).HasColumnName("CategoryCode").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            builder.HasOne(c => c.Company).WithMany().HasForeignKey(p => p.CompanyCode).IsRequired();
        }
    }
}
