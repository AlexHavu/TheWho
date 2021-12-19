using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic.Documents;

namespace Tipalti.TheWho.Services
{
    public interface IJiraService
    {
        public IEnumerable<ResourceDocumentResult> GetIssuesPerTeamAsResources(int id, IEnumerable<string> Domains);
    }
}
