using HtmlAgilityPack;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Confluence.Models;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Enums;
using Tipalti.TheWho.Models.Confluence;
using Tipalti.TheWho.Models.Confluence.RestApiResponse;
using Tipalti.TheWho.Services;
using Tipalti.Utils.Result;


namespace Tipalti.TheWho.Dal.Confluence
{
    public class ConfluenceRepository: IConfluenceRepository
    {
        private readonly HttpClient _client;
        private readonly ConfluenceConfiguration _confluenceConfiguration;
        private readonly IIndexerUtils _indexerUtils;
        private const string GetContentSearchApi = "content/search";
        private const string GetChildPages = "child/page";
        private const string GetContentApi = "content";
        private const string ConfluenceRestPath = "/rest/api/";
        private const string ExpandBodyQueryParam = "expand=body.view";
        private const int PageSize = 25;
        private readonly List<string> _teamNames;

        public ConfluenceRepository(HttpClient client, 
            ConfluenceConfiguration confluenceConfiguration,
            IIndexerUtils indexerUtils)
        {
            _client = client;
            _confluenceConfiguration = confluenceConfiguration;
            _client.SetBearerToken(_confluenceConfiguration.Token);
            _indexerUtils = indexerUtils;
            _teamNames = _indexerUtils.GetTeamNames();
        }

        public async Task<Result<IEnumerable<ResourceDocument>>> GetPagesAsync(IEnumerable<string> spaces, IEnumerable<string> domains)
        {
            string spacesExpression = buildSpacesExpression(spaces);
            string textExpression = buildDomainsExpression(domains);
            int totalPages = 0;
            int totalSize;

            var results = new List<ResourceDocument>();
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
                    return Result<IEnumerable<ResourceDocument>>.CreateFailResult(result.FailureReason);
                }
            }
            while (totalPages < totalSize);
            return Result<IEnumerable<ResourceDocument>>.CreateSuccessResult(results);
        }

        private ResourceDocument ConvertToModel(ConfluenceResult confluenceResult)
        {
            return new ResourceDocument
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
            string path = @$"{_confluenceConfiguration.BaseUrl}{ConfluenceRestPath}{GetContentSearchApi}?cql=type=page and {spacesExpression} and ({textExpression})&expand=body.view&limit={PageSize}&start={start}";
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

        private string getTeamFromBody(string bodyHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(bodyHtml);

            var htmlNode = doc.DocumentNode.SelectSingleNode("//strong[text()='Owner:']");
            if (htmlNode != null)
            {
                string owner = htmlNode.ParentNode.NextSibling.InnerHtml;
                if (!owner.Contains("<")) // filter out if we are holding an element
                {
                    if (owner.IndexOf("(") > 0)
                    {
                        owner = owner.Substring(0, owner.IndexOf("("));                                                
                    }
                    string teamFromList = _teamNames.Where(x => owner.Contains(x)).FirstOrDefault();
                    return teamFromList ?? owner;

                }
            }
            return string.Empty;
        }

        private string getDescriptionFromBody(string bodyHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(bodyHtml);

            var htmlNode = doc.DocumentNode.SelectSingleNode("//strong[text()='Description: ']");
            if (htmlNode != null)
            {
                string desc = htmlNode.ParentNode.NextSibling.InnerHtml;
                if (!desc.Contains("<")) // filter out if we are holding an element
                {
                    return desc;
                }
            }
            return string.Empty;
        }

        private async Task<string> GetServiceOwner(ChildPageModel page)
        {
            string path = @$"{_confluenceConfiguration.BaseUrl}{ConfluenceRestPath}{GetContentApi}/{page.Id}?{ExpandBodyQueryParam}";
            
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                PageModel result = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
                if (!result.Title.Contains("Template"))
                {
                    return getTeamFromBody(result.Body.View.Value);
                }
            }
            return null;
        }

        private async Task<string> GetServiceDescription(ChildPageModel page)
        {
            string path = @$"{_confluenceConfiguration.BaseUrl}{ConfluenceRestPath}{GetContentApi}/{page.Id}?{ExpandBodyQueryParam}";

            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {   
                PageModel result = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
                if (!result.Title.Contains("Template"))
                {   
                    return getDescriptionFromBody(result.Body.View.Value);
                }
            }
            return null;
        }

        private async Task<IEnumerable<ServiceDocument>> GetServices(string servicesRootId)
        {            
            string path = @$"{_confluenceConfiguration.BaseUrl}{ConfluenceRestPath}{GetContentApi}/{servicesRootId}/{GetChildPages}";
            
            var results = new List<ServiceDocument>();
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                ChildPagesModel result = JsonConvert.DeserializeObject<ChildPagesModel>(await response.Content.ReadAsStringAsync());

                foreach (ChildPageModel item in result.results)
                {
                    string owner = await GetServiceOwner(item);
                    string description = await GetServiceDescription(item);
                    if (!String.IsNullOrEmpty(owner))
                    {
                        results.Add(new ServiceDocument()
                        {
                            Name = item.Title,
                            Id = item.Id,
                            Owner = owner,
                            Description = description
                        });
                    }
                }                
            }

            return results;
        }

        public async Task<Result<IEnumerable<ServiceDocument>>> GetServicesAsync(string servicesRootId)
        {       
            return Result<IEnumerable<ServiceDocument>>.CreateSuccessResult(await GetServices(servicesRootId));
        }
    }
}
