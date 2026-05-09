using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TrungTamLapTrinh.Web.Models;

namespace TrungTamLapTrinh.Web.Data;

public partial class TrungTamLapTrinhContext : DbContext
{
    public TrungTamLapTrinhContext(DbContextOptions<TrungTamLapTrinhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CenterClass> CenterClasses { get; set; }

    public virtual DbSet<ClassStatus> ClassStatuses { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseLevel> CourseLevels { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }


    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A649A29993");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E4CA8733FD").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053443BCC4D4").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<CenterClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__CenterCl__CB1927C0BEAECCC4");

            entity.ToTable("CenterClass");

            entity.Property(e => e.ClassId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ClassName).HasMaxLength(250);
            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.RoomName).HasMaxLength(100);
            entity.Property(e => e.TeacherId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Course).WithMany(p => p.CenterClasses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CenterClass_Course");

            entity.HasOne(d => d.Status).WithMany(p => p.CenterClasses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CenterClass_Status");

            entity.HasOne(d => d.Teacher).WithMany(p => p.CenterClasses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CenterClass_Teacher");
        });

        modelBuilder.Entity<ClassStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__ClassSta__C8EE2063D5CD3894");

            entity.ToTable("ClassStatus");

            entity.HasIndex(e => e.StatusCode, "UQ__ClassSta__6A7B44FC239BFFB9").IsUnique();

            entity.Property(e => e.StatusCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A7B4875715");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CourseName).HasMaxLength(250);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.Level).WithMany(p => p.Courses)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_CourseLevel");
        });

        modelBuilder.Entity<CourseLevel>(entity =>
        {
            entity.HasKey(e => e.LevelId).HasName("PK__CourseLe__09F03C26E6A7C74A");

            entity.ToTable("CourseLevel");

            entity.HasIndex(e => e.LevelCode, "UQ__CourseLe__F57705F8CCEC9D1C").IsUnique();

            entity.Property(e => e.LevelCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LevelName).HasMaxLength(100);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F68771BA9EC347F");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => new { e.StudentId, e.CourseId }, "UQ_Enrollment_Student_Course").IsUnique();

            entity.Property(e => e.ApprovedByTeacherId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ClassId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.StudentId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.ApprovedByTeacher).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ApprovedByTeacherId)
                .HasConstraintName("FK_Enrollment_ApprovedTeacher");

            entity.HasOne(d => d.Class).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Enrollment_Class");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Course");

            entity.HasOne(d => d.Status).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Status");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<EnrollmentStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Enrollme__C8EE2063A85A9A45");

            entity.ToTable("EnrollmentStatus");

            entity.HasIndex(e => e.StatusCode, "UQ__Enrollme__6A7B44FCC3E656B9").IsUnique();

            entity.Property(e => e.StatusCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });


        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A38E262A372");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.EnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Enrollment");

            entity.HasOne(d => d.Method).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Method");

            entity.HasOne(d => d.Status).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Status");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__FC68185171CA536C");

            entity.ToTable("PaymentMethod");

            entity.HasIndex(e => e.MethodCode, "UQ__PaymentM__11E9210D563F3271").IsUnique();

            entity.Property(e => e.MethodCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MethodName).HasMaxLength(100);
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__PaymentS__C8EE2063C5FF3BEA");

            entity.ToTable("PaymentStatus");

            entity.HasIndex(e => e.StatusCode, "UQ__PaymentS__6A7B44FC9FC02E95").IsUnique();

            entity.Property(e => e.StatusCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A040F5150");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "UQ__Role__8A2B6160B375941B").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B49A3AA0BDB");

            entity.ToTable("Schedule");

            entity.Property(e => e.ClassId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RoomName).HasMaxLength(100);

            entity.HasOne(d => d.Class).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_CenterClass");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99CE7042DA");

            entity.ToTable("Student");

            entity.HasIndex(e => e.AccountId, "UQ__Student__349DA5A7F318A561").IsUnique();

            entity.Property(e => e.StudentId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Account");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teacher__EDF259643B7624F3");

            entity.ToTable("Teacher");

            entity.HasIndex(e => e.AccountId, "UQ__Teacher__349DA5A71E257147").IsUnique();

            entity.Property(e => e.TeacherId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Specialization).HasMaxLength(150);

            entity.HasOne(d => d.Account).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
