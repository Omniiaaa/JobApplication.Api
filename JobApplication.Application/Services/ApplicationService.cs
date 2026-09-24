using Hangfire;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class ApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IJobRepository jobRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _backgroundJobClient = backgroundJobClient;
        }
       
        public async Task<bool> UpdateStatus( int id,JobApplicationStatus newStatus)
        {
            var application = await _applicationRepository.GetByIdAsync(id);

            if (application == null)
                return false;

            if (newStatus <= application.JobApplicationStatus)
                return false;

            application.JobApplicationStatus = newStatus;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Cancel(int id, int candidateId)
        {
            var application = await _applicationRepository.GetByIdAsync(id);

            if (application == null)
                return false;

            if (application.CandidateId != candidateId)
                return false;

            if (application.JobApplicationStatus != JobApplicationStatus.Applied &&
                application.JobApplicationStatus != JobApplicationStatus.UnderReview)
                return false;

            application.JobApplicationStatus = JobApplicationStatus.Cancelled;
            application.CancelledAt = DateTime.UtcNow;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);

            await _applicationRepository.SaveChangesAsync();

            _backgroundJobClient.Enqueue<INotificationService>(
    x => x.NotifyCandidate(id));

            return true;
        }
        public async Task<int?> Apply(int jobId, int candidateId)
        {
            // 1. Check that the job exists and is active
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null || !job.IsActive)
                return null;

            // 2. Check if the candidate already applied
            var alreadyApplied = _applicationRepository.Get()
                .Any(a => a.JobId == jobId && a.CandidateId == candidateId);

            if (alreadyApplied)
                return null;

            // 3. Create application
            var application = new JobCandidateApplication
            {
                JobId = jobId,
                CandidateId = candidateId,
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
