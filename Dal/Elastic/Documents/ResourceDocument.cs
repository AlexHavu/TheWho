using System.Collections.Generic;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-resource")]
    public class ResourceDocument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RecourseType { get; set; }
        public string Content{ get; set; }
        public string Link { get; set; }
        public string Route { get; set; }
    }
}
