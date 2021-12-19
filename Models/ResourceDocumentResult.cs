using System.Collections.Generic;
using Tipalti.TheWho.Enums;

namespace Tipalti.TheWho.Models
{
    public class ResourceDocumentResult: BaseSearchResult
    {
        public string Title { get; set; }
        public eRecourseType RecourseType { get; set; }
        public List<string> Domains { get; set; }
        public string Link { get; set; }
    }
}
