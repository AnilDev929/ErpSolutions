using Microsoft.EntityFrameworkCore;
using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<AcademicYear> AcademicYears { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SchoolClass>(entity =>
            {
                entity.HasKey(e => e.ClassID);

                entity.Property(e => e.ClassName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Stream)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.DeptCode)
                      .HasMaxLength(10);
            });
        }
    }
}
