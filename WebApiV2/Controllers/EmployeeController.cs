using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiV2.Models;
using System.IO;

namespace WebApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EmployeeDBContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, EmployeeDBContext context, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await _context.Employees.ToListAsync());
        }


        [HttpPost]
        public async Task<JsonResult> Post(Employee emp)
        {
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return new JsonResult("Added successfully");
        }


        [HttpPut]
        public async Task<JsonResult> Put(int id, Employee emp)
        {
          

            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return new JsonResult("Employee not found");
            }

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            return new JsonResult("Deleted successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public async Task<JsonResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            IEnumerable<Department> alldeparts = _context.Departments;
            List<string> allDepartmentNames = new();
            foreach (var item in alldeparts)
            {
                allDepartmentNames.Add(item.DepartmentName);
            }

            return new JsonResult(allDepartmentNames);
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

    }
}
