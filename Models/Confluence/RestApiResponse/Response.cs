using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Confluence.RestApiResponse
{
    public class Response
    {
        public List<ConfluenceResult> results { get; set; }
        public int Size { get; set; }
        public int TotalSize { get; set; }
    }
}
