using System;

namespace Tipalti.TheWho.Dal.Elastic.Documents
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
