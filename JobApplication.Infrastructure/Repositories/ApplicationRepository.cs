using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Infrastructure.Repositories
{
    public class ApplicationRepository: IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<JobCandidateApplication> Get()
        {
            return _context.JobCandidateApplications;
        }

        public async Task<JobCandidateApplication?> GetByIdAsync(int id)
        {
            return await _context.JobCandidateApplications
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task InsertAsync(JobCandidateApplication application)
        {
            await _context.JobCandidateApplications.AddAsync(application);
        }
        public void Update(JobCandidateApplication application)
        {
            _context.JobCandidateApplications.Update(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

