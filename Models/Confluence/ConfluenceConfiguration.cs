using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Confluence
{
    public class ConfluenceConfiguration 
    {
        public const string Provider = "Confluence";
        public string BaseUrl { get; set; }
        public string Token { get; set; }
        public int HttpClientRetryCount { get; set; }
    }
}
