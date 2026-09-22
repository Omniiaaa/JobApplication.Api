using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Queries.GetAllJobs.Query
{
    public class GetAllJobHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<Job>>
    {
        private readonly IJobRepository _jobRepository;

        public GetAllJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<Job>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs =  _jobRepository.Get().ToList();
            return jobs;
        }
            
    }
}
