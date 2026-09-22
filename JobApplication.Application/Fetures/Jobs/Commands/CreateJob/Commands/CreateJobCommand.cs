using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Commands.CreateJob.Commands
{
    public class CreateJobCommand:IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
