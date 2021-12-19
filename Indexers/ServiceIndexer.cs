using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Confluence;
using Tipalti.TheWho.Dal.Confluence.Models;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Services;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public class ServiceIndexer : IServiceIndexer
    {
        private readonly IDbElasticTheWhoRepository _dbElasticTheWhoRepository;
        private readonly IConfluenceRepository _confluenceRepository;
        private readonly IIndexerUtils _indexerUtils;
        private const string servicesRootPageId = "47810265";

        public ServiceIndexer(IConfluenceRepository confluenceRepository,
                              IDbElasticTheWhoRepository dbElasticTheWhoRepository, 
                              IIndexerUtils indexerUtils)
        {
            _confluenceRepository = confluenceRepository;
            _dbElasticTheWhoRepository = dbElasticTheWhoRepository;
            _indexerUtils = indexerUtils;
        }

        public async Task<Result> RunAsync()
        {
            try
            {
                Result<IEnumerable<ServiceDocument>> result = await _confluenceRepository.GetServicesAsync(servicesRootPageId);
                if (!result.WasOperationSuccessful)
                {
                    return Result.CreateFailResult(result.FailureReason);
                }

                _dbElasticTheWhoRepository.BulkAddOrUpdate(result.OperationData.ToList());

                return Result.CreateSuccessResult();
            }
            catch (Exception e)
            {
                return Result.CreateFailResult($"Error during ServiceIndexer. error:{e.Message}");
            }
        }        
    }
}
