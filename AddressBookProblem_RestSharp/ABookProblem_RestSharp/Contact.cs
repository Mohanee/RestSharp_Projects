using System;
using System.Collections.Generic;
using System.Text;

namespace ABookProblem_RestSharp
{
    /// <summary>
    ///Modal class for Contact
    /// </summary>
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }

}
