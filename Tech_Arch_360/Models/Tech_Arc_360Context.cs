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

        public virtual DbSet<ActionMaster> ActionMasters { get; set; } = null!;
        public virtual DbSet<InventoryQuestionnaire> InventoryQuestionnaires { get; set; } = null!;
        public virtual DbSet<MenuMaster> MenuMasters { get; set; } = null!;
        public virtual DbSet<RoleMaster> RoleMasters { get; set; } = null!;
        public virtual DbSet<RoleMenuMaster> RoleMenuMasters { get; set; } = null!;
        public virtual DbSet<TenantMaster> TenantMasters { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserMaster> UserMasters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ELW5232;Database=Tech_Arc_360;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionMaster>(entity =>
            {
                entity.HasKey(e => e.ActionId)
                    .HasName("PK__ActionMa__FFE3F4B9BE9F5227");

                entity.ToTable("ActionMaster");

                entity.Property(e => e.ActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ActionID");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

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

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InventoryQuestionnaires)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_InventoryQuestionnaires_UserMaster");
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => e.MenuId)
                    .HasName("PK__MenuMast__C99ED250897E17C7");

                entity.ToTable("MenuMaster");

                entity.Property(e => e.MenuId)
                    .ValueGeneratedNever()
                    .HasColumnName("MenuID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ParentMenuId).HasColumnName("ParentMenuID");
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__RoleMast__8AFACE3AA2F9FB13");

                entity.ToTable("RoleMaster");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.ActionIds)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleMenuMaster>(entity =>
            {
                entity.HasKey(e => e.RoleMenuId)
                    .HasName("PK__RoleMenu__F86287962FAA5786");

                entity.ToTable("RoleMenuMaster");

                entity.Property(e => e.RoleMenuId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleMenuID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<TenantMaster>(entity =>
            {
                entity.HasKey(e => e.TenantId)
                    .HasName("PK__TenantMa__2E9B478133B398C3");

                entity.ToTable("TenantMaster");

                entity.Property(e => e.TenantId)
                    .ValueGeneratedNever()
                    .HasColumnName("TenantID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TenantName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.PasswordHash).HasMaxLength(200);

                entity.Property(e => e.Role).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserMast__1788CC4C49CE3A01");

                entity.ToTable("UserMaster");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserMasters)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserMaster_RoleMaster");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.UserMasters)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_UserMaster_TenantMaster");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
