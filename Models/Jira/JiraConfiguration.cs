using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Jira
{
    public class JiraConfiguration
    {
        public const string Provider = "Jira";
        public string BaseUrl { get; set; }
        public string Token { get; set; }
        public int HttpClientRetryCount { get; set; }
    }
}
