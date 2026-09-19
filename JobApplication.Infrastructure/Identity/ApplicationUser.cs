using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Infrastructure.Identity
{
    public class ApplicationUser:IdentityUser<int>
    {
        public int? CandidateId { get; set; }
        public int? RecruiterId { get; set; }

    }
}
