using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Jira.RestApiResponse
{
    public class GetIssuesResponse
    {
        public List<IssuesResult> Issues { get; set; }
        public int StartAt { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
    }
}
