using System.Collections.Generic;
using Nest;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-spaces")]
    public class SpacesDocument
    {
        public int Id { get; set; }
        public List<string> Spaces { get; set; }
    }
}
