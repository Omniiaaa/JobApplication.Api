using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Queries.GetJobById
{
    
    public class GetJobByIdHandler:IRequestHandler<GetJobByIdQuery, Job>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobByIdHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

         public async Task<Job> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = _jobRepository.Get().FirstOrDefault(j => j.Id == request.Id);
            return job;
        }
    }
}
