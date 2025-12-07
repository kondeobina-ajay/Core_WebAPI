using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Core_WebAPI.Models.Auth;

namespace Core_WebAPI.Models
{
    public class Department
    {

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; } = null!;

        public decimal Budget { get; set; }       // Keep this one

        public int ManagerId { get; set; }

        public int EmployeeCount { get; set; }

    }
}
