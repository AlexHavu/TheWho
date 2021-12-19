using System.Collections.Generic;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-resource")]
    public class ResourceDocument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RecourseType { get; set; }
        public List<DomainModel> Domains { get; set; }
        public string Link { get; set; }
    }

    public class DomainModel
    {
        public int DomainId { get; set; }
        public int Count { get; set; }
        public bool IsInTitleOrPath { get; set; }
    }
}
