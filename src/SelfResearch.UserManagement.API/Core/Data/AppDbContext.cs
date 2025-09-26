using System;
using System.Diagnostics.CodeAnalysis;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace SelfResearch.UserManagement.API.Core.Data;

[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.Entity<User>(entity =>
            {
                  entity.ToTable("users");
                  entity.HasKey(e => e.Id).HasName("pk_users");

                  entity.Property(e => e.Id)
                    .HasColumnName("id");

                  entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired();

                  entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired();
            });
      }
}
