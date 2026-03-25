using Microsoft.EntityFrameworkCore;
using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }

        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<StudentHostelAllocation> StudentHostelAllocations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
