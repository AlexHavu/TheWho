using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using Tipalti.TheWho.Models.Bamboo;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public class TeamIndexer: ITeamIndexer
    {
        private readonly IDbElasticTheWhoRepository _dbElasticTheWhoRepository;

        public TeamIndexer(IDbElasticTheWhoRepository dbElasticTheWhoRepository)
        {
            _dbElasticTheWhoRepository = dbElasticTheWhoRepository;
        }

        public async Task<Result> RunAsync(string root)
        {
            try
            {
                var path = Path.Combine(root, @"ExternalFiles\employees.json");
                StreamReader file = new StreamReader(path);
                string json = await file.ReadToEndAsync();
                Employees employeesResponse = JsonConvert.DeserializeObject<Employees>(json);
                IEnumerable<Employee> employees = employeesResponse.employees;
                ILookup<int?, Employee> employeesByTeamLeader = employees.Where(x=>x.SupervisorEId.HasValue)
                                                                         .ToLookup(x => x.SupervisorEId);
                IEnumerable<TeamConfigurationDocument> teamConfigurations = _dbElasticTheWhoRepository.GetTeams().Values;
                List<TeamDocument> teamDocuments = new List<TeamDocument>();
                foreach (var teamConfiguration in teamConfigurations)
                {
                    int teamLeaderId = teamConfiguration.TeamLeaderId;
                    Employee employee = employees.FirstOrDefault(x => x.Id == teamLeaderId);
                    if(employee == null)
                    {
                        //could not find team leader
                        continue;
                    }

                    var teamMembers = employeesByTeamLeader.Where(x => x.Key == teamLeaderId)
                                      .SelectMany(x => x).Select(x => ConvertToTeamMember(x)).ToList();
                    var teamDoc = new TeamDocument
                    {
                        TeamLeader = ConvertToTeamMember(employee),
                        TeamName = teamConfiguration.TeamName,
                        TeamMembers = teamMembers,
                        Confluence = teamConfiguration.TeamSpace,
                        Jira = teamConfiguration.JiraBoardLink
                    };
                    teamDocuments.Add(teamDoc);
                }
                _dbElasticTheWhoRepository.BulkAddOrUpdate(teamDocuments);
                return Result.CreateSuccessResult();
            }
            catch(Exception e)
            {
                return Result.CreateFailResult($"Failed to index employees error:{e.Message}");
            }
        }

        private TeamMemberModel ConvertToTeamMember(Employee employee)
        {
            return new TeamMemberModel
            {
                Email = employee.BestEmail,
                Name = $"{employee.FirstName} {employee.LastName}",
                Phone = employee.WorkPhone,
                Title = employee.JobTitle
            };
        }
    }
}
