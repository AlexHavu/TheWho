using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models.Jira;
using Tipalti.TheWho.Models.Jira.RestApiResponse;
using Tipalti.TheWho.Models.Confluence.RestApiResponse;
using Tipalti.Utils.Result;
using Newtonsoft.Json;

namespace Tipalti.TheWho.Dal.Jira
{
    public class JiraRepository : IJiraReposiroty
    {
        private readonly HttpClient _client;
        private readonly JiraConfiguration _jiraConfiguration;
        private const string GetIssuesApi = "/issue";
        private const string GetBoardApi = "/board";
        private const string JiraRestPath = "/rest/agile/1.0";
        private const int PageSize = 50;

        public JiraRepository(HttpClient client,
            JiraConfiguration jiraConfiguration)
        {
            _client = client;
            _jiraConfiguration = jiraConfiguration;
            _client.SetBearerToken(_jiraConfiguration.Token);
        }

        public async Task<Result<IEnumerable<IssuesResult>>> GetIssuesForTeamBoardAsync(int teamBoardId)
        {           
            int totalPages = 0;
            int totalSize;

            var results = new List<IssuesResult>();
            do
            {
                Result<GetIssuesResponse> result = await GetIssues(teamBoardId, totalPages);
                if (result.WasOperationSuccessful)
                {
                    GetIssuesResponse response = result.OperationData;
                    totalPages += response.MaxResults;
                    totalSize = response.Total;
                    results.AddRange(response.Issues);
                }
                else
                {
                    return Result<IEnumerable<IssuesResult>>.CreateFailResult(result.FailureReason);
                }
            }
            while (totalPages < totalSize);

            return Result<IEnumerable<IssuesResult>>.CreateSuccessResult(results);
        }

        private async Task<Result<GetIssuesResponse>> GetIssues(int teamBoardId, int startIndex)
        {
            string path = @$"{_jiraConfiguration.BaseUrl}{JiraRestPath}{GetBoardApi}/{teamBoardId}{GetIssuesApi}?startAt={startIndex}";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var srtingResult = await response.Content.ReadAsStringAsync();
                    GetIssuesResponse result = JsonConvert.DeserializeObject<GetIssuesResponse>(await response.Content.ReadAsStringAsync());
                    return Result<GetIssuesResponse>.CreateSuccessResult(result);
                }

                return Result<GetIssuesResponse>.CreateFailResult($"Api failed with status code: {response.StatusCode}");
            }
            catch (Exception e)
            {
                return Result<GetIssuesResponse>.CreateFailResult($"Api failed error: {e.Message}");
            }
        }     
    }
}
