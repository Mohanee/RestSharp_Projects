using EmployeeProblem_RestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayroll_RESTSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;
        [TestInitialize]

        ///Setting up the client
        public void setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// Requesting the json-server to send the stored data
        /// </summary>
        /// <returns>resuls returned after execution</returns>
        private IRestResponse GetEmployeeList()
        {
            // Request the client by giving resource url and method to perform
            IRestRequest request = new RestRequest("/employees", Method.GET);

            // Executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Method to test for display of all the employees using Get
        /// </summary>
        [TestMethod]
        public void OnCallingAPI_RetrieveAllElementsFromJSONServer()
        {
            IRestResponse response = GetEmployeeList();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(7, employeeList.Count);

            foreach (var item in employeeList)
            {
                Console.WriteLine("id: " + item.id + "\tName: " + item.name + "\tSalary: " + item.salary);
            }
        }

        /// <summary>
        /// Method to test the working of POST command to add a new employee
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);

            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Kristine");
            jObjectbody.Add("Salary", "95000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

            Employee employeeList = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Kristine", employeeList.name);
            Assert.AreEqual(95000, employeeList.salary);
        }

        /// <summary>
        /// Method to test adding multiple employees at a time using POST Command
        /// </summary>
        [TestMethod]
        public void GivenMultipleEmployees_WhenPosted_ShouldReturnEmployeeListWithAddedEmployees()
        {
            List<Employee> list = new List<Employee>();
            list.Add(new Employee { name = "Sweta Basu", salary = 75000 });
            list.Add(new Employee { name = "Nikhil Mahajan", salary = 65000 });
            list.Add(new Employee { name = "Sreyansh Bhandari", salary = 75000 });
            foreach (Employee employee in list)
            {
                RestRequest request = new RestRequest("/employees/create", Method.POST);

                JObject jObject = new JObject();
                jObject.Add("name", employee.name);
                jObject.Add("salary", employee.salary);
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name, dataResponse.name);
                Assert.AreEqual(employee.salary, dataResponse.salary);
            }
        }

        /// <summary>
        /// Method to test working of PUT command to update an employee
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            RestRequest request = new RestRequest("employees/5", Method.PUT);

            JObject jobject = new JObject();
            jobject.Add("name", "Nishant");
            jobject.Add("salary", 60000);
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
         
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(dataResponse.salary, 60000);
        }


        /// <summary>
        /// Method to check working of DELETE command to delete an employee
        /// </summary>
        [TestMethod]
        public void GivenEmployee_WhenDeleted_ShouldReturnStatusOk()
        {
            RestRequest request = new RestRequest("/Employees/7", Method.DELETE);
        
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}