using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Command.ApplyApplication
{
    public class ApplyApplicationHandler : IRequestHandler<ApplyApplicationCommand, int?>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;

        public ApplyApplicationHandler(
            IApplicationRepository applicationRepository,
            IJobRepository jobRepository)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
        }

        public async Task<int?> Handle(ApplyApplicationCommand request, CancellationToken cancellationToken)
        {
            // 1. Check that the job exists and is active
            var job = await _jobRepository.GetByIdAsync(request.JobId);

            if (job == null || !job.IsActive)
                return null;

            // 2. Check if the candidate already applied
            var alreadyApplied = _applicationRepository.Get()
                .Any(a => a.JobId == request.JobId && a.CandidateId == request.CandidateId);

            if (alreadyApplied)
                return null;

            // 3. Create application
            var application = new JobCandidateApplication
            {
                JobId = request.JobId,
                CandidateId = request.CandidateId,
                JobApplicationStatus = JobApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow,
                StatusUpdatedAt = DateTime.UtcNow
            };

            // 4. Save
            await _applicationRepository.InsertAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return application.Id;
        }
    }
}
