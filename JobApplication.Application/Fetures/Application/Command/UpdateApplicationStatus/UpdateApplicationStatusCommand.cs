using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Command.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public JobApplicationStatus NewStatus { get; set; }
    }
}
