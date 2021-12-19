using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models;

namespace Tipalti.TheWho.Services
{
    public class SearchService : ISearchService
    {
        private IDbElasticTheWhoRepository _elasticDB;

        public SearchService(IDbElasticTheWhoRepository dbMongoTheWhoRepository )
        {
            _elasticDB = (DbElasticTheWhoRepository)dbMongoTheWhoRepository;
        }
        public bool SearchResults(string search)
        {
            //List<BaseSearchResult> allResults = new List<BaseSearchResult>();
            //List<TeamDocument> teamResult = GetTeams(search);
            //if(teamResult == null && !teamResult.Any())
            //{
            //    TeamDocument myTeam = null;
            //   var teamConfig = _elasticDB.GetTeamConfiguartion();
            //    foreach(var team in teamConfig)
            //    {
            //        if(team.Domains.Contains(search))
            //        {
            //            var name = team.TeamName;
            //            myTeam = _elasticDB;
            //            break;
            //        }
            //    }

            //}
            //List<ResourceDocumentResult> resourceDocumentResult = GetResources(domainID);
          



         //   allResults.AddRange(resourceDocumentResult);
           // allResults.AddRange(teamResult);
            return true;
        }

        private List<TeamDocument> GetTeams(int domainID)
        {
            var resourceModel = _elasticDB.GetResourceDocumentsByDomain(domainID.ToString());
            if (resourceModel == null)
            {
                return new List<TeamDocument>();
            }

            List<TeamDocument> searchRsult = resourceModel.Select(res =>
            new TeamDocument
            {
                DocumentType = eDocumentType.Team,
                Id = res.Id
            }
            ).ToList();

            return searchRsult;
        }

        private List<ResourceDocumentResult> GetResources(int domainID)
        {
            var resourceModel = _elasticDB.GetResourceDocumentsByDomain(domainID.ToString());
            if(resourceModel == null)
            {
                return new List<ResourceDocumentResult>();
            }

            List<ResourceDocumentResult> searchRsult = resourceModel.Select(res =>
            new ResourceDocumentResult
            {
                DocumentType = res.RecourseType == 1 ? eDocumentType.JiraRecourse : eDocumentType.Confluence,
                Domains = new List<string>(),
                Id = res.Id,
                Link = res.Link,
                Title = res.Title
            }
            ).ToList();

            return searchRsult;
        }
    }
}
