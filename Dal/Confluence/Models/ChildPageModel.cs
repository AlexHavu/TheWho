using System.Collections.Generic;

namespace Tipalti.TheWho.Dal.Confluence.Models
{ 
    public class ChildPagesModel
    {
        public List<ChildPageModel> results { get; set; }
    }

    public class ChildPageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
