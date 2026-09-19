using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces
{
    public interface ICandidateRepository
    {

        Task InsertAsync(Candidate candidate);
        void Update(Candidate candidate);
        IQueryable<Candidate> Get();
        void Remove(Candidate candidate);
        Task SaveChangesAsync();
    }
}
