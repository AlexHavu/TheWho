using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models.Confluence.RestApiResponse
{
    public class ConfluenceResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Body Body { get; set; }

        [JsonProperty("_links")]
        public Links Links { get; set; }
    }
}
