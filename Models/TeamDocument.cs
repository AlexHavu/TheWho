using System.Collections.Generic;

namespace Tipalti.TheWho.Models
{ 
  
    public class TeamDocument:BaseSearchResult
    {
        public string Confluence { get; set; }
        public List<string> Domains { get; set; }
        public string Name { get; set; }
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
    }
}
