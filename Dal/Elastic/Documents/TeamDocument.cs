using System.Collections.Generic;
using Nest;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-team")]
    [ElasticsearchType(IdProperty = nameof(TeamName))]
    public class TeamDocument
    {
        public string Confluence { get; set; }
        public string Jira { get; set; }
        public string TeamName { get; set; }
        public List<string> Services { get; set; }
        public string Slack { get; set; }
        public TeamMemberModel TeamLeader { get; set; }
        public List<TeamMemberModel> TeamMembers { get; set; }
    }

    public class TeamMemberModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
