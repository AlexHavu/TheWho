using System;

namespace Tipalti.TheWho.Documents
{
    public class IndexNameAttribute: Attribute
    {
        public string IndexName { get; set; }
        public IndexNameAttribute(string indexName)
        {
            IndexName = indexName;
        }
    }
}
