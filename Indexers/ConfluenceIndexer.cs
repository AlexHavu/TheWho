using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Confluence;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public class ConfluenceIndexer : IConfluenceIndexer
    {
        private readonly IDbElasticTheWhoRepository _dbElasticTheWhoRepository;
        private readonly IConfluenceRepository _confluenceRepository;

        public ConfluenceIndexer(IConfluenceRepository confluenceRepository,
                                 IDbElasticTheWhoRepository dbElasticTheWhoRepository)
        {
            _confluenceRepository = confluenceRepository;
            _dbElasticTheWhoRepository = dbElasticTheWhoRepository;
        }

        public async Task<Result> RunAsync()
        {
            //TODO: get list of spaces and domains from the DB
            var domains = new string[] { "MultiFx", "MultiFx Bai" };
            var spaces = new string[] { "TT", "KBTES" };

            try
            {
                Result<IEnumerable<ResourceDocumentResult>> result = await _confluenceRepository.GetPagesAsync(spaces, domains);
                if(!result.WasOperationSuccessful)
                {
                    return Result.CreateFailResult(result.FailureReason);
                }

                _dbElasticTheWhoRepository.BulkAddOrUpdate(result.OperationData.ToList());

                return Result.CreateSuccessResult();
            }
            catch(Exception e)
            {
                return Result.CreateFailResult($"Error during ConfluenceIndexer. error:{e.Message}");
            }
        }
    }
}
