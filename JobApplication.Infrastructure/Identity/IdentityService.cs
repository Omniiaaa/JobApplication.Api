using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace JobApplication.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<bool> Register(RegisterDto dto)
        {
            
            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole<int>(dto.Role));
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                dto.Password);

            if (!result.Succeeded)
                return false;
            if (dto.Role == "Candidate")
            {
                var candidate = new Candidate
                {
                    Name = dto.Name
                };

                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();

                user.CandidateId = candidate.Id;
                await _userManager.UpdateAsync(user);
            }
            else if (dto.Role == "Recruiter")
            {
                var recruiter = new Recruiter
                {
                    Name = dto.Name
                };

                _context.Recruiters.Add(recruiter);
                await _context.SaveChangesAsync();

                user.RecruiterId = recruiter.Id;
                await _userManager.UpdateAsync(user);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return true;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                dto.Password);

            if (!passwordValid)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["Jwt:DurationInMinutes"]!)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    public async Task<int?> GetCandidateId(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user?.CandidateId;
        }
        public async Task<int?> GetRecruiterId(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user?.RecruiterId;
        }
    }
    }