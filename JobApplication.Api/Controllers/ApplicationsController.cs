using JobApplication.Application.Interfaces;
using JobApplication.Application.Services;
using JobApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        private readonly IIdentityService _identityService;

        public ApplicationsController(
            ApplicationService applicationService,
            IIdentityService identityService)
        {
            _applicationService = applicationService;
            _identityService = identityService;
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, JobApplicationStatus newStatus)
        {
            var result = await _applicationService.UpdateStatus(id, newStatus);

            if (!result)
                return BadRequest();

            return Ok();
        }
        [Authorize(Roles = "Candidate")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userIdValue == null)
                return Unauthorized();

            var userId = int.Parse(userIdValue);

            var candidateId = await _identityService.GetCandidateId(userId);

            if (candidateId == null)
                return BadRequest("Candidate profile not found.");

            var result = await _applicationService.Cancel(
                id,
                candidateId.Value);

            if (!result)
                return BadRequest("Application cannot be cancelled.");

            return Ok("Application cancelled successfully.");
        }
        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> Apply(int jobId)
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userIdValue == null)
                return Unauthorized();

            var userId = int.Parse(userIdValue);

            var candidateId = await _identityService.GetCandidateId(userId);

            if (candidateId == null)
                return BadRequest("Candidate profile not found.");

            var id = await _applicationService.Apply(
                jobId,
                candidateId.Value);

            if (id == null)
                return BadRequest("Application cannot be created.");

            return Ok(new { id });
        }
    }
}