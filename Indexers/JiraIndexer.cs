using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Dal.Jira;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models.Jira;
using Tipalti.TheWho.Models.Jira.RestApiResponse;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public class JiraIndexer : IJiraIndexer
    {
        private readonly IDbElasticTheWhoRepository _dbElasticTheWhoRepository;
        private readonly IJiraReposiroty _jiraRepository;
        private readonly JiraConfiguration _jiraConfiguration;

        public JiraIndexer(IJiraReposiroty jiraRepository,
                           IDbElasticTheWhoRepository dbElasticTheWhoRepository,
                           JiraConfiguration jiraConfiguration)
        {
            _jiraRepository = jiraRepository;
            _dbElasticTheWhoRepository = dbElasticTheWhoRepository;
            _jiraConfiguration = jiraConfiguration;
        }

        public async Task<Result> RunAsync()
        {
            IEnumerable<TeamConfigurationDocument> teams = _dbElasticTheWhoRepository.GetTeams().Values;
            List<ResourceDocument> documentsToAdd = new List<ResourceDocument>();

            foreach (TeamConfigurationDocument team in teams)
            {
                Result<IEnumerable<IssuesResult>> result = await _jiraRepository.GetIssuesForTeamBoardAsync(team.JiraBoardId);
                if (!result.WasOperationSuccessful)
                {
                    return Result.CreateFailResult(result.FailureReason);
                }

                documentsToAdd.AddRange(GetRelevantIssuesAsResources(result.OperationData, team.Domains));
            }

            _dbElasticTheWhoRepository.BulkAddOrUpdate(documentsToAdd);
            return Result.CreateSuccessResult();
        }

        private IEnumerable<ResourceDocument> GetRelevantIssuesAsResources(IEnumerable<IssuesResult> issues, IEnumerable<string> domains)
        {
            List<ResourceDocument> results = new List<ResourceDocument>();

            foreach (IssuesResult issue in issues)
            {
                if (domains.Any(domain => issue.Fields.Summary != null && issue.Fields.Summary.ToLower().Contains(domain.ToLower())) ||
                    domains.Any(domain => issue.Fields.Description != null && issue.Fields.Description.ToLower().Contains(domain.ToLower())))
                {
                    results.Add(ConvertToModel(issue));
                }
            }

            return results;
        }

        private ResourceDocument ConvertToModel(IssuesResult issuesResult)
        {
            return new ResourceDocument { 
                Id = issuesResult.Id,
                RecourseType = (int)eRecourseType.Jira,
                Content = issuesResult.Fields.Description,
                Link = $"{_jiraConfiguration.BaseUrl}/browse/{issuesResult.Key}",
                Title = issuesResult.Fields.Summary
            };
        }
    }
}
