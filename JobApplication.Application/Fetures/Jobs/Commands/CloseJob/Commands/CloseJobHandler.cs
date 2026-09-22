using JobApplication.Application.Interfaces;
using JobApplication.Application.Services;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Commands.CloseJob.Commands
{
   
  
        public class CloseJobHandler
            : IRequestHandler<CloseJobCommand, bool>
        {
            private readonly JobService _jobService;

            public CloseJobHandler(JobService jobService)
            {
                _jobService = jobService;
            }

            public async Task<bool> Handle(
                CloseJobCommand request,
                CancellationToken cancellationToken)
            {
                return await _jobService.Close(
                    request.id,
                    request.recruiterId);
            }
        }
    }
