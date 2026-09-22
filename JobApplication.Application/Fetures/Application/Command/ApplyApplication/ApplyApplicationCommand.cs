using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Command.ApplyApplication
{
    public class ApplyApplicationCommand : IRequest<int?>
    {
        public int JobId { get; set; }
        public int CandidateId { get; set; }
    }
}
