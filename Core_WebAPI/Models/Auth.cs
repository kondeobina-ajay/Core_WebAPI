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

            public string email { get; set; } = null;// PK
            public string Username { get; set; } = null!;  // login username
            public string PasswordHash { get; set; } = null!; // store hashed password
            public string? PhoneNumber { get; set; }  
            public string? JwtToken { get; set; }
            public DateTime? JwtTokenExpiry { get; set; }
        }
        public class Employee
        {
            [Key]
            public int Id { get; set; } // PK
            public string Username { get; set; } = null!;  // login username
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Department { get; set; }
            public string? Position { get; set; }
            public DateTime? HireDate { get; set; }
            public decimal? Salary { get; set; }
            public string? Status { get; set; } // active | inactive | on-leave
            public string? Address { get; set; }
            public string? EmergencyContact { get; set; }

        }


    }
    
}
