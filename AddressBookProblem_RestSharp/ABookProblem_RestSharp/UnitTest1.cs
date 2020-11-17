using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace ABookProblem_RestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        /// <summary>
        /// Initialising the client with corresponding local host
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// Method to get stored data as a result when requested to JSON Server
        /// </summary>
        /// <returns></returns>
        private IRestResponse getContactList()
        {
            RestRequest request = new RestRequest("/contacts", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// method to display all the contacts present in the addressBook json file
        /// </summary>
        [TestMethod]
        public void OnClaiingGETApi_ReturnContactList()
        {
            IRestResponse response = getContactList();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            List<Contact> dataresponse = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(5, dataresponse.Count);
            foreach (var item in dataresponse)
                System.Console.WriteLine("ID: " + item.ID + " Name: " + item.Name + " Address: " + item.Address + " City: " + item.City + " State: " + item.State + " Zip: " + item.Zip + " Phone: " + item.Phone + " Email: " + item.Email);
        }

        /// <summary>
        /// Method to test work of POST command to add a new Contact to addressBook JSON file
        /// </summary>
        [TestMethod]
        public void GivenMultipleContacts_OnPost_ShouldReturnCountOfContacts()
        {
            RestRequest request = new RestRequest("/contacts", Method.POST);
            JObject[] jObjectbody = new JObject[2];

            JObject obj = new JObject();
            obj.Add("Name", "Monima");
            obj.Add("Address", "21/F,Rahat Colony");
            obj.Add("City", "Patna");
            obj.Add("State", "Bihar");
            obj.Add("Zip", 987612);
            obj.Add("Phone", "7802345718");
            obj.Add("Email", "monima23@email.com");
            jObjectbody[0] = obj;

            obj = new JObject();
            obj.Add("Name", "Nikita");
            obj.Add("Address", "AlphaBlock-H");
            obj.Add("City", "Bombay");
            obj.Add("State", "Maharashtra");
            obj.Add("Zip", 236715);
            obj.Add("Phone", "765239845");
            obj.Add("Email", "nikita45@email.com");
            jObjectbody[1] = obj;
            for (int i = 0; i < 2; i++)
            {
                request.AddParameter("application/json", jObjectbody[i], ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

                Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
                if (i == 0)
                {
                    Assert.AreEqual("Name new", dataResponse.Name);
                    Assert.AreEqual("Address new", dataResponse.Address);
                }
            }
        }

        /// <summary>
        /// Method to test working of PUT command to update a contact by his name
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPatch_ShouldReturnUpdatedEmployee()
        {
            RestRequest request = new RestRequest("/contacts/2", Method.PATCH);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("Name", "Rekhansh");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Coder Bhai", dataResponse.Name);
        }
    }
}