using Microsoft.AspNetCore.Mvc;
using Thales.DAL;
using Thales.Models;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Thales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly string apiUrl;

        public WebApiController()
        {
            apiUrl = "https://dummy.restapiexample.com/api/v1/";
        }
        // GET: api/WebApi
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            DALEmployee _dal = new DALEmployee(apiUrl + "employees");
            ResponseEmployees responseEmployees = new ResponseEmployees();
            responseEmployees = await _dal.GetList();
            if (responseEmployees.status == "200")
            {
                return Ok(responseEmployees);
            }
            else {
                return BadRequest(responseEmployees);
            }
            
        }

        // GET api/WebApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            DALEmployee _dal = new DALEmployee(apiUrl + "employee/"+id);
            ResponseEmployees responseEmployees = new ResponseEmployees();
            responseEmployees = await _dal.GetRegister();
            if (responseEmployees.status == "200")
            {
                return Ok(responseEmployees);
            }
            else
            {
                return BadRequest(responseEmployees);
            }
        }
    }
}
