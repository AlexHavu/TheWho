using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Enums;

namespace Tipalti.TheWho.Models
{
 
    public class AllResult 
    {
        public int Id { get; set; }
        public eDocumentType DocumentType { get; set; }
        public string Title { get; set; }
        public eRecourseType RecourseType { get; set; }
        public List<string> Domains { get; set; }
        public string Link { get; set; }
        public string Jira { get; set; }

        public string Confluence { get; set; }
      
        public string Name { get; set; }
        public List<string> Services { get; set; }
        public string Slack { get; set; }
        public TeamMemberModel TeamLeader { get; set; }
        public List<TeamMemberModel> TeamMembers { get; set; }

    }
}
