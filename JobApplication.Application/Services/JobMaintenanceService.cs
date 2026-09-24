using JobApplication.Application.Interfaces;

namespace JobApplication.Application.Services
{
    public class JobMaintenanceService
    {
        private readonly IJobRepository _jobRepository;

        public JobMaintenanceService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task AutoCloseJobs()
        {
            var expirationDate = DateTime.UtcNow.AddDays(-30);

            var jobs = _jobRepository
                .Get()
                .Where(j =>
                    j.IsActive &&
                    j.CreatedAt.HasValue &&
                    j.CreatedAt.Value <= expirationDate)
                .ToList();

            foreach (var job in jobs)
            {
                job.IsActive = false;
                job.ClosedAt = DateTime.UtcNow;
                job.ClosedBy = null;
            }

            await _jobRepository.SaveChangesAsync();
        }
    }
}
