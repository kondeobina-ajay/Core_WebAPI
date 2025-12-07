using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_WebAPI.Models
{
    public class Auth
    {
     
        public class Manager
        {
            [Key]
            public int Id { get; set; } // PK

            [Column(TypeName = "varchar(255)")]
            public string email { get; set; } = null;// PK

            [Column(TypeName = "varchar(100)")]
            public string Username { get; set; } = null!;  // login username

            [Column(TypeName = "varchar(500)")]
            public string PasswordHash { get; set; } = null!; // store hashed password

            [Column(TypeName = "varchar(20)")]
            public string? PhoneNumber { get; set; }
            [Column(TypeName = "text")]
            public string? JwtToken { get; set; }
            public DateTime? JwtTokenExpiry { get; set; }
        }
        public class Employee
        {
            [Key]
            public int Id { get; set; } // PK

            [Column(TypeName = "varchar(100)")]
            public string Username { get; set; } = null!;  // login username

            [Column(TypeName = "varchar(100)")]
            public string? FirstName { get; set; }
            [Column(TypeName = "varchar(100)")]
            public string? LastName { get; set; }
            [Column(TypeName = "varchar(255)")]
            public string? Email { get; set; }
            [Column(TypeName = "varchar(20)")]
            public string? Phone { get; set; }

            [Column(TypeName = "varchar(100)")]
            public string? Department { get; set; }
            [Column(TypeName = "varchar(100)")]
            public string? Position { get; set; }
            public DateTime? HireDate { get; set; }
            public decimal? Salary { get; set; }
            [Column(TypeName = "varchar(50)")]
            public string? Status { get; set; } // active | inactive | on-leave

            [Column(TypeName = "text")]
            public string? Address { get; set; }

            [Column(TypeName = "varchar(255)")]
            public string? EmergencyContact { get; set; }

        }


    }
    
}
