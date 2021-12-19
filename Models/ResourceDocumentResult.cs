using System.Collections.Generic;

namespace Tipalti.TheWho.Models
{


    public class ResourceDocumentResult: BaseSearchResult
    {
        public string Title { get; set; }
        public int RecourseType { get; set; }
        public List<DomainModel> Domains { get; set; }
        public string Link { get; set; }

        public override void SetPrevew()
        {
           
        }


    }

    public class DomainModel
    {
        public int DomainId { get; set; }
        public int Count { get; set; }
        public bool IsInTitleOrPath { get; set; }
    }
}
