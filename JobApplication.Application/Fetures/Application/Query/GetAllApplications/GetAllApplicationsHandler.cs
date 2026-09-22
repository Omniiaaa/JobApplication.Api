using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Query.GetAllApplications
{
    public class GetAllApplicationsHandler : IRequestHandler<GetAllApplicationsQuery, IEnumerable<JobCandidateApplication>>
    {
        private readonly IApplicationRepository _applicationRepository;

        public GetAllApplicationsHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public Task<IEnumerable<JobCandidateApplication>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = _applicationRepository.Get().ToList().AsEnumerable();
            return Task.FromResult(applications);
        }
    }
}
