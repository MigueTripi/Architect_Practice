using Microsoft.EntityFrameworkCore;
using SelfResearch.Financial.API.Feature.Wallet;

namespace SelfResearch.Financial.API.Core.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.ToTable("wallets");

            entity.HasKey(e => e.Id)
                .HasName("pk_wallets");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("balance")
                .IsRequired();

            entity.Property(e => e.State)
                .HasColumnName("state")
                .IsRequired();

            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasColumnName("currency")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");
        });

        base.OnModelCreating(modelBuilder);
    }   
}
