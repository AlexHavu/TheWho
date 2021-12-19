using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Bamboo
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string WorkPhone { get; set; }
        public string BestEmail { get; set; }
        public string Supervisor { get; set; }
        public int? SupervisorEId { get; set; }
    }
}
