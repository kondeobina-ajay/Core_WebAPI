using Core_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports()
        {
            // Employees
            var employees = await _context.Employees
                .Select(e => new
                {
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Phone,
                    e.Department,
                    e.Position,
                    e.HireDate,
                    e.Salary,
                    e.Status,
                    e.Address,
                    e.EmergencyContact
                })
                .ToListAsync();

            // Attendance
            var attendance = await _context.Attendance
                .Include(a => a.Employee)
                .Select(a => new
                {
                    a.Id,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    Date = a.Date.ToString("yyyy-MM-dd"),
                    a.CheckIn,
                    a.CheckOut,
                    a.Status,
                    a.HoursWorked
                })
                .ToListAsync();

            var departments = await _context.Departments
                    .Join(
                        _context.Employees,
                        d => d.Name,
                        e => e.Department,
                        (d, e) => new { d.Id, d.Name, d.Budget, Employee = e }
                    )
                    .GroupBy(x => new { x.Id, x.Name, x.Budget })
                    .Select(g => new
                    {
                        g.Key.Id,
                        g.Key.Name,
                        EmployeeCount = g.Count(),
                        g.Key.Budget
                    })
                    .OrderBy(x => x.Id)
                    .ToListAsync();



            return Ok(new
            {
                employees,
                attendance,
                departments
            });
        }
    }
}
