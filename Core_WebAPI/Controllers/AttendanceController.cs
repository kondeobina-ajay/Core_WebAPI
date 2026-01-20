using Core_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Core_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance()
        {
            var attendance = await _context.Attendance
                     .Include(a => a.Employee) // Include employee info
                     .Select(a => new
                     {
                         a.Id,
                         EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName, // or a.Employee.Username
                         Date = a.Date.ToString("yyyy-MM-dd"), // to match HTML date input format
                         a.CheckIn,
                         a.CheckOut,
                         a.Status,
                         a.HoursWorked
                     })
         .ToListAsync();

            return Ok(attendance);
        }
    }
}
