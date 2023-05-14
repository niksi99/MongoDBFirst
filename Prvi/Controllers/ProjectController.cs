using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prvi.Models.Db;
using Prvi.Models.Entities;
using Prvi.Services;

namespace Prvi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService projectService;
        private readonly EmployeeService employeeService;
        public ProjectController(ProjectService projectService, EmployeeService employeeService)
        {
            this.projectService = projectService;
            this.employeeService = employeeService;
        }

        [Route("CreateNewProject")]
        [HttpPost]
        public async Task<IActionResult> CreateNewProject(Project p)
        {
            if (p == null) return BadRequest();

            await this.projectService.CreateNewProject(p);
            return Ok(p);
        }

        [Route("AddEmployeeToProject/{jmbg}/{projectName}")]
        [HttpPost]
        public async Task<IActionResult> AddEmployeeToProject(string jmbg, string projectName)
        {
            Project pr = await this.projectService.GetProjectByProjName(projectName);
            if (pr == null) return BadRequest("Nepostojeci projekat");

            Employee empl = await this.employeeService.GetEmployeeByJMBG(jmbg);
            if (empl == null) return BadRequest("Nepostojeci zaposleni");

            pr.EmployeesOnProject.Add(empl);

            await this.projectService.UpdateAsync(pr.Id, pr);
            return Ok("Dodato");
        }

        [Route("GetAllProjects")]
        [HttpGet]
        public async Task<IActionResult> GetAllProjectsWithEmployyesOnThem()
        {
            return Ok(await this.projectService.GetAllProject());
        }

        [Route("GetAllEmployeesOnAllProjects")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesOnAllProjects()
        {
            return Ok(this.projectService.GetAllEmployeesOnProjects());
        }

        [Route("GetProjectByEmployeeJMBG/{jmbg}")]
        [HttpGet]
        public async Task<IActionResult> GetProjectByEmployeeJMBG(string jmbg)
        {
            return Ok(await this.projectService.GetProjectByEmployeeJMBG(jmbg));
        }
    }
}