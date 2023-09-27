using MemoryCacheDemo.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyDemoEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        ICacheService _cacheService;
        public EmployeeController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// Get All employee list
        /// </summary>
        /// <returns> List of EMployees </returns>
        [Route("GetEmployees")]
        [HttpGet]
        public List<Employee> GetEmployees()
        {
            List<Employee> getResponse = _cacheService.GetData<List<Employee>>("employee");
            return getResponse;
        }

        /// <summary>
        /// Insert a new Employee and assign Guid for each employee's Id
        /// </summary>
        /// <param name="emp"></param>
        /// <returns> Returns a success or failure message </returns>
        [Route("InsertEmployee")]
        [HttpPost]
        public string InsertEmployee([FromBody] Employee emp)
        {
            List<Employee> getResponse = new List<Employee>();
            getResponse = _cacheService.GetData<List<Employee>>("employee");
            if (getResponse == null)
                getResponse = new List<Employee>();

            emp.Id = Guid.NewGuid().ToString();
            getResponse.Add(emp);
            var response = _cacheService.SetData("employee", getResponse, DateTimeOffset.MaxValue);
            if (response)
            {
                return "Employee added successfully";
            }
            return "Request failed";
        }

        /// <summary>
        /// Update an existing Employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns> Returns a success or failure message </returns>
        [Route("UpdateEmployee")]
        [HttpPut]
        public string UpdateEmployee([FromBody] Employee emp)
        {
            List<Employee> getResponse = new List<Employee>();
            getResponse = _cacheService.GetData<List<Employee>>("employee");
            if (getResponse == null)
                getResponse = new List<Employee>();

            var existingEmp = getResponse.Where(x => x.Id == emp.Id).FirstOrDefault();
            existingEmp.Name = emp.Name;
            existingEmp.Salary = emp.Salary;

            //getResponse.Add(existedEmp);
            var response = _cacheService.SetData("employee", getResponse, DateTimeOffset.MaxValue);
            if (response)
            {
                return "Employee updated successfully";
            }
            return "Request failed";
        }

        /// <summary>
        /// Remove employee from a list
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Returns a success or failure message </returns>
        [Route("RemoveEmployee")]
        [HttpDelete]
        public string RemoveEmployee([FromBody] string id)
        {
            List<Employee> getResponse = new List<Employee>();
            getResponse = _cacheService.GetData<List<Employee>>("employee");
            if (getResponse == null)
                getResponse = new List<Employee>();

            var existedEmp = getResponse.Where(x => x.Id == id).FirstOrDefault();
            getResponse.Remove(existedEmp);
            var response = _cacheService.SetData("employee", getResponse, DateTimeOffset.MaxValue);
            if (response)
            {
                return "Employee removed successfully";
            }
            return "Request failed";
        }

    }
}
