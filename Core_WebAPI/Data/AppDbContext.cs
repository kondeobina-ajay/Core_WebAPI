using Core_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using static Core_WebAPI.Models.Auth;

namespace Core_WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Manager> Managers { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;

        public DbSet<Attendance> Attendance { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee Salary
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            // Attendance HoursWorked
            modelBuilder.Entity<Attendance>()
                .Property(a => a.HoursWorked)
                .HasColumnType("decimal(5,2)");

            // Department Budget
            modelBuilder.Entity<Department>()
                .Property(d => d.Budget)
                .HasColumnType("decimal(18,2)");

            // Attendance FK (optional fix)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
