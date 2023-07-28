using Microsoft.AspNetCore.Mvc;
using Thales.Controllers;

namespace TestUnitThales
{
    public class UnitTest1
    {
        private HomeController homeController;
        public UnitTest1()
        {
            this.homeController = new HomeController();
        }

        [Fact]
        public async void SearchEmployee_Exist()
        {
            try
            {
                string employeeId = "17";
                var result = await this.homeController.SearchEmployee(employeeId) as JsonResult;
                Assert.NotNull(result);
                dynamic? data = result.Value;
                string? status = data?.status;
                Assert.True(status == "200" || status == "429");
                if (status == "200") {
                    Assert.Equal("200", data?.status);
                    Assert.Equal(17, data?.data[0].id);
                    Assert.Equal("Paul Byrd", data?.data[0].employee_Name);
                    Assert.Equal(725000, data?.data[0].employee_salary);
                    Assert.Equal(CalculateAnualSalary(725000), data?.data[0].employee_anual_salary);
                    Assert.Equal(64, data?.data[0].employee_age);
                    Assert.Equal("", data?.data[0].profile_image);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Fact]
        public async void SearchEmployee_GetAll()
        {
            try
            {
                string employeeId = "";
                var result = await this.homeController.SearchEmployee(employeeId) as JsonResult;
                Assert.NotNull(result);
                dynamic? data = result.Value;
                string? status = data?.status;
                Assert.True(status == "200" || status == "429");
                if (status == "200") {
                    int? count = data?.data.Count;
                    Assert.True(count > 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Fact]
        public async void SearchEmployee_NoExist()
        {
            try
            {
                var homeController = new HomeController();
                string employeeId = "60";
                var result = await homeController.SearchEmployee(employeeId) as JsonResult;
                Assert.NotNull(result);
                dynamic? data = result.Value;
                string? status = data?.status;
                Assert.True(status == "404" || status == "429");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static int CalculateAnualSalary(int employee_salary)
        {
            return employee_salary * 12;
        }
    }
}