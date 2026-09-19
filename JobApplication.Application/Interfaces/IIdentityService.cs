using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JobApplication.Application.DTOs;

namespace JobApplication.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> Register(RegisterDto dto);
        Task<string?> Login(LoginDto dto);
        Task<int?> GetCandidateId(int userId);
        Task<int?> GetRecruiterId(int userId);
    }
}