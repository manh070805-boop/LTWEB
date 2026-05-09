using Microsoft.EntityFrameworkCore;
using qlkh.Models;

namespace qlkh.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // AUTH
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }

        // USER
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }   

        // COURSE
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<Course> Courses { get; set; }

    
        public DbSet<CenterClass> CenterClasses { get; set; }   
        public DbSet<ClassStatus> ClassStatuses { get; set; } 

    
        public DbSet<Lesson> Lessons { get; set; }   
        public DbSet<Schedule> Schedules { get; set; }   

       
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }

      
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    }
}
