using System.ComponentModel.DataAnnotations;
using static Core_WebAPI.Models.Auth;

namespace Core_WebAPI.Models
{
    public class Department
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Budget { get; set; }       // Keep this one

        public int ManagerId { get; set; }

        public int EmployeeCount { get; set; }

    }
}
