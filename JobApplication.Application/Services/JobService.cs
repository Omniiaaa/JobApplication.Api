using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class JobService
    {

            private readonly IJobRepository _jobRepository;

            public JobService(IJobRepository jobRepository)
            {
                _jobRepository = jobRepository;
            }

            public async Task<int> CreateAsync(CreateJobDto createJobDto)
            {
                var job = new Job()
                {
                    Title = createJobDto.Title,
                    Description = createJobDto.Description,
                    IsActive = true
                };
                await _jobRepository.InsertAsync(job);
                await _jobRepository.SaveChangesAsync();

                return job.Id;
            }
        public async Task<bool> Close(int jobId, int recruiterId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                return false;

            // Only the owning recruiter can close the job
            if (job.RecruiterId != recruiterId)
                return false;

            // Job must be active
            if (!job.IsActive)
                return false;

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = recruiterId;

            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync();

            return true;
        }
    }
    }


