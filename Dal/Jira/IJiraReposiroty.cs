using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Models.Jira.RestApiResponse;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Dal.Jira
{
    public interface IJiraReposiroty
    {
        Task<Result<IEnumerable<IssuesResult>>> GetIssuesForTeamBoardAsync(int teamBoardId);
    }
}
