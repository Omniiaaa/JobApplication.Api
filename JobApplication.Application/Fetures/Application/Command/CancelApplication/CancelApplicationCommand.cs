using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Command.CancelApplication
{
    public class CancelApplicationCommand:IRequest<bool>
    {
        public int id { get; set; }
    }
}
