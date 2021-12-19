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
            Dictionary<string, TeamDocument> teams = _esRepository.GetTeams();
            return new List<string>(teams.Keys);
        }
    }
}
