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
        public List<AllResult> SearchResults(string search)
        {
            List<TeamConfigurationDocument> teamConfig = _elasticDB.GetTeams().Values.ToList();
            var domains = _elasticDB.GetDomains();
            var dicTeamToDomains = teamConfig.ToDictionary(i => i.TeamName, v => v.Domains);
            List<AllResult> allResults = new List<AllResult>();

            var isTeam = _elasticDB.GetDocumentById<TeamDocument>(search);
            if (isTeam != null)
            {
                var myDomains = dicTeamToDomains[isTeam.TeamName];
                allResults.Add(BuildTeamModel(isTeam, myDomains));
                foreach (var domain in myDomains)
                {
                    List<Models.AllResult> resourceDocumentResult = GetResources(domain);
                    allResults.AddRange(resourceDocumentResult);
                }
            }

            else if (domains.Contains(search))
            {
                allResults.Add(GetTeamByDomain(search, teamConfig));
                allResults.AddRange(GetResources(search));

            }
            return allResults;
        }

        private AllResult BuildTeamModel(TeamDocument dbTeam, List<string> domains)
        {
            var services = _elasticDB.GetServiceByOwner(dbTeam.TeamName);
            AllResult teamDocumentResult = new AllResult
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
                TeamMembers = dbTeam.TeamMembers.Select(tm =>
                new Models.TeamMemberModel
                {
                    Name = tm.Name,
                    Title = tm.Title,
                    Image = tm.Image
                }).ToList(),
                Domains = domains
            };
            return teamDocumentResult;
        }

        private void TeamSearch(TeamDocumentResult teamResult, List<BaseSearchResult> allResults)
        {

        }

        private AllResult GetTeamByDomain(string search, List<TeamConfigurationDocument> teamConfig)
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

        private List<Models.AllResult> GetResources(string domainID)
        {
            var resourceModel = _elasticDB.GetResourceDocumentsByDomain(domainID);
            if (resourceModel == null)
            {
                return new List<Models.AllResult>();
            }

            List<Models.AllResult> searchRsult = resourceModel.Select(res =>
            new Models.AllResult
            {
                DocumentType = res.RecourseType == 1 ? eDocumentType.JiraRecourse : eDocumentType.ConfluenceRecourse,
                Domains = new List<string>(),
                Id = res.Id,
                Link = res.Link,
                Title = res.Title
            }
            ).ToList();

            List<Models.AllResult> jira = searchRsult.Where(r => r.DocumentType == eDocumentType.JiraRecourse).Take(5).ToList();
            List<Models.AllResult> Con = searchRsult.Where(r => r.DocumentType == eDocumentType.ConfluenceRecourse).Take(5).ToList();
            jira.AddRange(Con);


            return jira;
        }


    }
}
