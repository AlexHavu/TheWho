using System.Collections.Generic;
using Nest;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-team_configuration")]
    [ElasticsearchType(IdProperty = nameof(TeamName))]
    public class TeamConfigurationDocument
    {
        public string TeamName { get; set; }
        public int TeamLeaderId { get; set; }
        public int JiraBoardId { get; set; }
        public List<string> Domains { get; set; }
        public string TeamSpace { get; set; }
        public string JiraBoardLink { get; set; }
    }
}
