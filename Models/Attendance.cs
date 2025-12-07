using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Core_WebAPI.Models.Auth;

namespace Core_WebAPI.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }  // Primary key

        public int EmployeeId { get; set; }   // Link to Employee table
        public Employee Employee { get; set; }  // Navigation property

        [Column(TypeName = "varchar(255)")]
        public string EmployeeName { get; set; } = null!;

        public DateTime Date { get; set; }    // Attendance date

        public TimeSpan CheckIn { get; set; }     // Check-in time
        public TimeSpan CheckOut { get; set; }    // Check-out time

        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; } = null!;  // present, late, absent, half-day, etc.

        public decimal HoursWorked { get; set; }     // e.g., 8.5 hours
    }
}
