using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiV2.Models;

namespace WebApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly  EmployeeDBContext _context;

        public DepartmentController(IConfiguration configuration, EmployeeDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await _context.Departments.ToListAsync());
        }


        [HttpPost]
        public async Task<JsonResult> Post(Department dep)
        {
            _context.Departments.Add(dep);
            await _context.SaveChangesAsync();
            return new JsonResult("Added successfully");
        }


       // [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, Department dep)
        {
            if (id != dep.DepartmentID)
            {
                return new JsonResult("Failed successfully");
            }

            _context.Entry(dep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return new JsonResult("Failed successfully");
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var Department = await _context.Departments.FindAsync(id);
            if (Department == null)
            {
                return new JsonResult("Department not found");
            }

            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();

            return new JsonResult("Deleted successfully");
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }

    }
}
