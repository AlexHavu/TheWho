using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Models
{
    public class TeamConfigurationModel
    {
        public string TeamName { get; set; }
        public int TeamLeaderId { get; set; }
        public int JiraBoardId { get; set; }
        public List<string> Domains { get; set; }
    }
}
