using System.Collections.Generic;
using Tipalti.TheWho.Models;

namespace Tipalti.TheWho.Services
{
    public interface IIndexerUtils
    {
        public List<string> GetTeamNames();
        public Dictionary<string, TeamConfigurationModel> GetTeamsConfiguration(); 
    }
}
