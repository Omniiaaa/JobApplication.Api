using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Queries.GetAllJobs.Query
{
    public class GetAllJobsQuery:IRequest<IEnumerable<Job>>
    {

    }
}
