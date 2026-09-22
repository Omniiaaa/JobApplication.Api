using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Jobs.Commands.CloseJob.Commands
{
    public class CloseJobCommand:IRequest<bool>
    {
        public int id { get; set; }

        public int recruiterId { get; set; }
    }
}
