﻿using Microsoft.AspNetCore.Http;
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
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [Route("CreateNewEmployee")]
        [HttpPost]
        public async Task<IActionResult> CreateNewEmployee(Employee newEmp)
        {
            await this.employeeService.CreateNewEmployee(newEmp);
            return Ok(newEmp);
        }

        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await this.employeeService.GetAllEmployees());
        }

        [Route("GetOneEmployeeByName")]
        [HttpGet]
        public async Task<IActionResult> GetOneEmployeeByName(string firstName)
        {
            var provera = await this.employeeService.GetEmployeeByFirstName(firstName);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatName")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatName(string firstName)
        {
            var provera = await this.employeeService.GetAllEmployeesByName(firstName);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatNameAggr")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatNameAggr(string firstName)
        {
            var provera = await this.employeeService.GetAllEmployeesByNameAggr(firstName);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatNameIndexes/{firstName}")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatNameIndexes(string firstName)
        {
            var provera = await this.employeeService.GetAllEmployeesByNameIndexes(firstName);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatPayementIndexes/{payement}")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatPayementIndexes(int payement)
        {
            var provera = await this.employeeService.GetAllEmployeesByPayementIndexes(payement);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatPayementIndexes2/{payement}")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatPayementIndexes2(int payement)
        {
            var provera = await this.employeeService.GetAllEmpByPayment(payement);
            if (provera is null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllEmployeesByThatOccupationIndexes/{occupation}")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesByThatOccupationIndexes(string occupation)
        {
            var provera = await this.employeeService.GetAllEmpByOccup(occupation);
            if (provera == null)
                return BadRequest("Ne postoji zaposleni sa tim imenom");
            return Ok(provera);
        }

        [Route("GetAllPlata1")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlata1()
        {

            return Ok(await this.employeeService.GetAllEmployeesByPlataAggr());
        }

        [Route("GetAllPlata2/{fName}")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlata2(string fName)
        {

            return Ok(await this.employeeService.GetAllEmployeesByPlataAggrMatch(fName));
        }


    }
}