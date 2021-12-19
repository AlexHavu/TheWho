using System.Collections.Generic;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-spaces")]
    public class SpacesDocument
    {
        public List<string> Spaces { get; set; }
    }
}
