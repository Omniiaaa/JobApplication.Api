using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Query.GetApplicationById
{
    public class GetApplicationByIdQuery : IRequest<JobCandidateApplication?>
    {
        public int Id { get; set; }
    }
}
