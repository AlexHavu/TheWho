namespace Tipalti.TheWho.Dal.Confluence.Models
{
    public class PageModel
    {
        public string Id;
        public string Title;
        public Body Body;
    }

    public class View
    {
        public string Value { get; set; }
    }


    public class Body
    {
        public View View { get; set; }
    }
}
