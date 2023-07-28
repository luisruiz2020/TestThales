using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Thales.Models;

namespace Thales.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetListEmployees());
        }

        public async Task<ResponseEmployees> GetListEmployees()
        {
            ResponseEmployees? responseEmployees = new ResponseEmployees();
            _httpClient.BaseAddress = new Uri("https://localhost:7061/");
            HttpResponseMessage response = await _httpClient.GetAsync("api/WebApi");
            string temp = await response.Content.ReadAsStringAsync();
            responseEmployees = JsonConvert.DeserializeObject<ResponseEmployees>(temp);
            if (response.IsSuccessStatusCode)
            {
                responseEmployees?.data?.ForEach(x => x.employee_anual_salary = CalculateAnualSalary(x.employee_salary));
            }
            return responseEmployees;
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmployee(string employeeId)
        {
            ResponseEmployees? responseEmployees = new ResponseEmployees();

            if (String.IsNullOrEmpty(employeeId))
            {
                responseEmployees = await GetListEmployees();
            }
            else
            {
                _httpClient.BaseAddress = new Uri("https://localhost:7061/");
                HttpResponseMessage response = await _httpClient.GetAsync("api/WebApi/" + employeeId);
                string temp = await response.Content.ReadAsStringAsync();
                responseEmployees = JsonConvert.DeserializeObject<ResponseEmployees>(temp);
                if (response.IsSuccessStatusCode)
                {
                    responseEmployees?.data?.ForEach(x => x.employee_anual_salary = CalculateAnualSalary(x.employee_salary));
                }
            }
            return Json(responseEmployees);
        }

        public static int CalculateAnualSalary(int employee_salary)
        {
            return employee_salary * 12;
        }
    }
}