using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=ELW5232;Initial Catalog=Tech_Arc_360;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
