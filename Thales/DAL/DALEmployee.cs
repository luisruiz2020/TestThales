using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net.Http.Headers;
using System.Text.Json;
using Thales.Models;

namespace Thales.DAL
{
    public class DALEmployee
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient;

        public DALEmployee(string apiUrl)
        {
            this.apiUrl = apiUrl;
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<T> DeserializeApiResponse<T>(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<ResponseEmployees> GetList()
        {
            ResponseEmployees responseEmployees = new ResponseEmployees();
            
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}");
                response.EnsureSuccessStatusCode();
                responseEmployees = await DeserializeApiResponse<ResponseEmployees>(response);
                responseEmployees.status = ((int)response.StatusCode).ToString();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode.HasValue)
                {
                    responseEmployees.message = ex.Message;
                    responseEmployees.status = ((int)ex.StatusCode).ToString();
                }
                else {
                    responseEmployees.message = ex.Message;
                    responseEmployees.status = "999";
                }
            }

            return responseEmployees;
        }

        public async Task<ResponseEmployees> GetRegister()
        {
            ResponseEmployees responseEmployees = new ResponseEmployees();
            Employee data = new Employee();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}");
                response.EnsureSuccessStatusCode();
                ResponseEmployee responseIndividual = new ResponseEmployee();
                responseIndividual = await DeserializeApiResponse<ResponseEmployee>(response);
                if (responseIndividual.data != null)
                {
                    responseEmployees.status = ((int)response.StatusCode).ToString();
                    responseEmployees.message = responseIndividual.message;
                    responseEmployees.data = new List<Employee>();
                    responseEmployees.data.Add(responseIndividual.data);
                }
                else { 
                    throw new HttpRequestException("Employee not found", new Exception(), System.Net.HttpStatusCode.NotFound);
                }
                
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode.HasValue)
                {
                    responseEmployees.message = ex.Message;
                    responseEmployees.status = ((int)ex.StatusCode).ToString();
                }
                else
                {
                    responseEmployees.message = ex.Message;
                    responseEmployees.status = "999";
                }
            }

            return responseEmployees;
        }
    }
}
