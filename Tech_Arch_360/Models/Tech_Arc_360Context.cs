using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tech_Arch_360.Models
{
    public partial class Tech_Arc_360Context : DbContext
    {
        public Tech_Arc_360Context()
        {
        }

        public Tech_Arc_360Context(DbContextOptions<Tech_Arc_360Context> options)
            : base(options)
        {
        }

        public virtual DbSet<InventoryQuestionnaire> InventoryQuestionnaires { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=ELW5232;Database=Tech_Arc_360;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryQuestionnaire>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__Inventor__0DC06F8CD191A690");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.AnsweredBy).HasMaxLength(100);

                entity.Property(e => e.AnsweredOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastAnswerModifiedBy).HasMaxLength(100);

                entity.Property(e => e.LastAnswerModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Question).HasMaxLength(500);

                entity.Property(e => e.TenantId).HasColumnName("TenantID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.PasswordHash).HasMaxLength(200);

                entity.Property(e => e.Role).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
