using Core_WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core_WebAPI.Models.Auth;

namespace Core_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                .Select(e => new Employee
                {
                    Id = e.Id,
                    Username = e.Username,
                    FirstName = e.FirstName ?? "",
                    LastName = e.LastName ?? "",
                    Email = e.Email ?? "",
                    Phone = e.Phone ?? "",
                    Department = e.Department ?? "",
                    Position = e.Position ?? "",
                    HireDate = e.HireDate,
                    Salary = e.Salary,
                    Status = e.Status ?? "active",
                    Address = e.Address ?? "",
                    EmergencyContact = e.EmergencyContact ?? ""
                }).ToListAsync();
        }

        // GET: api/employee/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return employee;
        }

        // POST: api/employee
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return Ok(employee);
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        // POST: api/employee/bulk-delete
        [HttpPost("bulk-delete")]
        public async Task<IActionResult> DeleteMultipleEmployees([FromBody] int[] ids)
        {
            var employees = await _context.Employees.Where(e => ids.Contains(e.Id)).ToListAsync();
            _context.Employees.RemoveRange(employees);
            await _context.SaveChangesAsync();
            return Ok(employees);
        }

    }
}
