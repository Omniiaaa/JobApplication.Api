using JobApplication.Application.DTOs;
using JobApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobApplication.Application.Interfaces;

namespace JobApplication.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _JobService;

        private readonly IIdentityService _identityService;

        public JobsController(
            JobService jobService,
            IIdentityService identityService)
        {
            _JobService = jobService;
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
            var id = await _JobService.CreateAsync(createJobDto);
            return Ok(new
            {
                id = id
            });
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userIdValue == null)
                return Unauthorized();

            var userId = int.Parse(userIdValue);

            var recruiterId = await _identityService.GetRecruiterId(userId);

            if (recruiterId == null)
                return BadRequest("Recruiter profile not found.");

            var result = await _JobService.Close(
                id,
                recruiterId.Value);

            if (!result)
                return BadRequest("Job cannot be closed.");

            return Ok("Job closed successfully.");
        }
    }

}
