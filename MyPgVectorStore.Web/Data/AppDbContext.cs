using Microsoft.EntityFrameworkCore;
using MyPgVectorStore.Web.Models;

namespace MyPgVectorStore.Web.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Recommendation> Recommendations { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.ToTable("recommendations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            entity.Property(x => x.Title).HasColumnName("title").IsRequired();
            entity.Property(x => x.Category).HasColumnName("category").IsRequired();
            entity.Property(x => x.Embedding).HasColumnName("embedding").HasColumnType("vector(1024)").IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            entity.Property(x => x.Title).HasColumnName("title").IsRequired();
            entity.Property(x => x.Category).HasColumnName("category").IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary").IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").IsRequired();
        });
    }
}