using JobApplication.Application.Fetures.Application.Command.ApplyApplication;
using JobApplication.Application.Fetures.Application.Command.CancelApplication;
using JobApplication.Application.Fetures.Application.Command.UpdateApplicationStatus;
using JobApplication.Application.Fetures.Application.Query.GetAllApplications;
using JobApplication.Application.Fetures.Application.Query.GetApplicationById;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public ApplicationsController(
            IMediator mediator,
            IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _mediator.Send(new GetAllApplicationsQuery());
            return Ok(applications);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var application = await _mediator.Send(new GetApplicationByIdQuery { Id = id });

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, JobApplicationStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateApplicationStatusCommand
            {
                Id = id,
                NewStatus = newStatus
            });

            if (!result)
                return BadRequest("Application status could not be updated.");

            return Ok("Application status updated successfully.");
        }

        [Authorize(Roles = "Candidate")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _mediator.Send(new CancelApplicationCommand { id = id });

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

            var id = await _mediator.Send(new ApplyApplicationCommand
            {
                JobId = jobId,
                CandidateId = candidateId.Value
            });

            if (id == null)
                return BadRequest("Application cannot be created.");

            return Ok(new { id });
        }
    }
}