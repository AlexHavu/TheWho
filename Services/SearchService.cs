using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models;

namespace Tipalti.TheWho.Services
{
    public class SearchService : ISearchService
    {
        private IDbElasticTheWhoRepository _elasticDB;

        public SearchService(IDbElasticTheWhoRepository dbMongoTheWhoRepository)
        {
            _elasticDB = (DbElasticTheWhoRepository)dbMongoTheWhoRepository;
        }
        public List<BaseSearchResult> SearchResults(string search)
        {
            var teamConfig = _elasticDB.GetTeams().Values;
            var domains = _elasticDB.GetDomains();
            var dicTeamToDomains = teamConfig.ToDictionary(i => i.TeamName, v => v.Domains);
            List<BaseSearchResult> allResults = new List<BaseSearchResult>();

            var isTeam = _elasticDB.GetDocumentById<TeamDocument>(search);
            if (isTeam != null)
            {
                var myDomains = dicTeamToDomains[isTeam.TeamName];
                allResults.Add(BuildTeamModel(isTeam, myDomains));
                foreach (var domain in myDomains)
                {
                    List<Models.ResourceDocumentResult> resourceDocumentResult = GetResources(search);
                    allResults.AddRange(resourceDocumentResult);
                }



            }
            return allResults;
        }

        private TeamDocumentResult BuildTeamModel(TeamDocument dbTeam, List<string> domains)
        {
            var services = _elasticDB.GetServiceByOwner(dbTeam.TeamName);
            TeamDocumentResult teamDocumentResult = new TeamDocumentResult
            {
                DocumentType = eDocumentType.Team,
                Confluence = dbTeam.Confluence,
                Name = dbTeam.TeamName,
                Slack = dbTeam.Slack,
                Services = services.Select(s => s.Name).ToList(),
                TeamLeader = new Models.TeamMemberModel
                {
                    Name = dbTeam.TeamLeader.Name,
                    Title = dbTeam.TeamLeader.Title,
                    Image = dbTeam.TeamLeader.Image
                },
                // TeamMembers = dbTeam.TeamMembers,
                Domains = domains
            };
            return teamDocumentResult;
        }

        private void TeamSearch(TeamDocumentResult teamResult, List<BaseSearchResult> allResults)
        {

        }

        private TeamDocumentResult GetTeamByDomain(string search, List<TeamConfigurationDocument> teamConfig)
        {
            
            TeamDocument myTeam = null;
            List<string> myDomain = null;
            foreach (var teamConfigItem in teamConfig)
            {
                if (teamConfigItem.Domains.Contains(search))
                {
                    var name = teamConfigItem.TeamName;
                    myTeam = _elasticDB.GetDocumentById<TeamDocument>(name);
                    myDomain = teamConfigItem.Domains;
                    break;
                }
            }

            return BuildTeamModel(myTeam, myDomain);

        }

        private List<Models.ResourceDocumentResult> GetResources(string domainID)
        {
            var resourceModel = _elasticDB.GetResourceDocumentsByDomain(domainID);
            if (resourceModel == null)
            {
                return new List<Models.ResourceDocumentResult>();
            }

            List<Models.ResourceDocumentResult> searchRsult = resourceModel.Select(res =>
            new Models.ResourceDocumentResult
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
