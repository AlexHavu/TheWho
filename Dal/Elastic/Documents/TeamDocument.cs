using System.Collections.Generic;

namespace Tipalti.TheWho.Dal.Elastic.Documents
{
    [IndexName("the-who-team")]
    public class TeamDocument
    {
        public int Id { get; set; }
        public string Confluence { get; set; }
        public string Jira { get; set; }
        public List<string> Domains { get; set; }
        public string Name { get; set; }
        public List<string> Services { get; set; }
        public string Slack { get; set; }
        public TeamMemberModel TeamLeader { get; set; }
        public List<TeamMemberModel> TeamMembers { get; set; }
    }

    [IndexName("the-who-team_configuration")]
    public class TeamDocumentConfiguration
    {   
        public string TeamName { get; set; }        
    }

    public class TeamMemberModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
