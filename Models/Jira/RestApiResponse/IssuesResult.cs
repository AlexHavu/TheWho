using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Jira.RestApiResponse
{
    public class IssuesResult
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public Fields Fields { get; set; }
    }
}
