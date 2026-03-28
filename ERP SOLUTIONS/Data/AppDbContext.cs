using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MenuSection> MenuSections { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }

        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<StudentHostelAllocation> StudentHostelAllocations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
         
        public DbSet<RoleMenuAccess> RoleMenuAccess { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<MenuSection>()
            //    .HasMany(s => s.Items)
            //    .WithOne(i => i.MenuSections)
            //    .HasForeignKey(i => i.SectionId);

            modelBuilder.Entity<UserRole>()
       .HasKey(ur => ur.UserRoleID);

            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserID, ur.RoleID })
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID);

        }
    }
}
