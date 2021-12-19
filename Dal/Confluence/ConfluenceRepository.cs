using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models.Confluence;
using Tipalti.TheWho.Models.Confluence.RestApiResponse;
using Tipalti.Utils.Result;


namespace Tipalti.TheWho.Dal.Confluence
{
    public class ConfluenceRepository: IConfluenceRepository
    {
        private readonly HttpClient _client;
        private readonly ConfluenceConfiguration _confluenceConfiguration;
        private const string GetContentSearchApi = "content/search";
        private const string ConfluenceRestPath = "/rest/api/";
        private const int PageSize = 25;

        public ConfluenceRepository(HttpClient client, ConfluenceConfiguration confluenceConfiguration)
        {
            _client = client;
            _confluenceConfiguration = confluenceConfiguration;
            _client.SetBearerToken(_confluenceConfiguration.Token);
        }

        public async Task<Result<IEnumerable<ResourceDocumentResult>>> GetPagesAsync(IEnumerable<string> spaces, IEnumerable<string> domains)
        {
            string spacesExpression = buildSpacesExpression(spaces);
            string textExpression = buildDomainsExpression(domains);
            int totalPages = 0;
            int totalSize;

            var results = new List<ResourceDocumentResult>();
            do
            {
                Result<Response> result = await GetPages(spacesExpression, textExpression, totalPages);
                if (result.WasOperationSuccessful)
                {
                    Response response = result.OperationData;
                    totalPages += response.Size;
                    totalSize = response.TotalSize;
                    results.AddRange(response.results.Select(x => ConvertToModel(x)));
                }
                else
                {
                    return Result<IEnumerable<ResourceDocumentResult>>.CreateFailResult(result.FailureReason);
                }
            }
            while (totalPages < totalSize);
            return Result<IEnumerable<ResourceDocumentResult>>.CreateSuccessResult(results);
        }

        private ResourceDocumentResult ConvertToModel(ConfluenceResult confluenceResult)
        {
            return new ResourceDocumentResult
            {
                Id = confluenceResult.Id,
                RecourseType = (int)eRecourseType.Confluence,
                Content = confluenceResult.Body.View.Value,
                Link = $"{_confluenceConfiguration.BaseUrl}{confluenceResult.Links.Webui}",
                Title = confluenceResult.Title
            };
        }

        private async Task<Result<Response>> GetPages(string spacesExpression, string textExpression, int start)
        {
            string path = @$"{_confluenceConfiguration.BaseUrl}{ConfluenceRestPath}{GetContentSearchApi}?cql=type=page and {spacesExpression} and {textExpression}&expand=body.view&limit={PageSize}&start={start}";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    Response result = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                    return Result<Response>.CreateSuccessResult(result);
                }

                return Result<Response>.CreateFailResult($"Api failed with status code: {response.StatusCode}");
            }
            catch (Exception e)
            {
                return Result<Response>.CreateFailResult($"Api failed error: {e.Message}");
            }
        }

        private static string buildSpacesExpression(IEnumerable<string> spaces)
        {
            return $"space in ({String.Join(",", spaces)})";
        }

        private string buildDomainsExpression(IEnumerable<string> domains)
        {
            return $"text~'{String.Join("' or text~'", domains)}'";
        }
    }

}
