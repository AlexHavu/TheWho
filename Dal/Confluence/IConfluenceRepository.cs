using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Dal.Confluence
{
    public interface IConfluenceRepository
    {
        Task<Result<IEnumerable<ResourceDocumentResult>>> GetPagesAsync(IEnumerable<string> spaces, IEnumerable<string> domains);
        Task<Result<IEnumerable<ServiceDocument>>> GetServicesAsync(string servicesRootId);
    }
}
