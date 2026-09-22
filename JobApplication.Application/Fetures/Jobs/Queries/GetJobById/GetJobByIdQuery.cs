using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Queries.GetJobById
{
    public class GetJobByIdQuery:IRequest<Job>
    {
        public int Id { get; set; }
    }
}
