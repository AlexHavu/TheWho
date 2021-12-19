using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Models;

namespace Tipalti.TheWho.Services
{
    public class IndexerUtils : IIndexerUtils
    {
        IDbElasticTheWhoRepository _esRepository;

        public IndexerUtils(IDbElasticTheWhoRepository esRepo)
        {
            _esRepository = esRepo;
        }

        public List<string> GetTeamNames()
        {
            Dictionary<string, TeamConfigurationModel> teams = GetTeamsConfiguration();
            return new List<string>(teams.Keys);
        }

        public Dictionary<string, TeamConfigurationModel> GetTeamsConfiguration()
        {
            Dictionary<string, TeamConfigurationModel> result = new Dictionary<string, TeamConfigurationModel>();
            foreach (var item in _esRepository.GetTeams().Values)
            {
                result.Add(item.TeamName, new TeamConfigurationModel()
                {
                    Domains = item.Domains,
                    JiraBoardId = item.JiraBoardId,
                    TeamLeaderId = item.TeamLeaderId,
                    TeamName = item.TeamName
                });
            }

            return result;
        }
    }
}
