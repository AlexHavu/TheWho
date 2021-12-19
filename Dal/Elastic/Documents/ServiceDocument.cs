namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-service")]
    public class ServiceDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
    }
}
