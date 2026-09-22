using JobApplication.Application.DTOs;
using JobApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobApplication.Application.Interfaces;
using MediatR;
using JobApplication.Application.Fetures.Jobs.Queries.GetAllJobs.Query;
using JobApplication.Application.Fetures.Jobs.Commands.CreateJob.Commands;
using JobApplication.Application.Fetures.Jobs.Queries.GetJobById;
using JobApplication.Application.Fetures.Jobs.Commands.CloseJob.Commands;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Identity;

namespace JobApplication.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
       
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

       //  private readonly IIdentityService _identityService;

        //public JobsController(
            
        //    IIdentityService identityService)
        //{
        //    //_JobService = jobService;
        //    _identityService = identityService;
        //}

        [HttpGet]
        public IActionResult GetAll()
        {
            // var jobs = _JobService.GetAll();
            var jobs = _mediator.Send(new GetAllJobsQuery());
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // var job = _JobService.GetByIdAsync(id);
            var job = _mediator.Send(new GetJobByIdQuery()  { Id = id } );

            if (job == null)
                return NotFound();

            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
          //  var id = await _JobService.CreateAsync(createJobDto);
          var id =_mediator.Send(new CreateJobCommand() { Title = createJobDto.Title, Description = createJobDto.Description });
            return Ok(new
            {
                id = id
            });
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
           // var userIdValue = User.FindFirstValue(
           //     ClaimTypes.NameIdentifier);

           // if (userIdValue == null)
           //     return Unauthorized();
           // var userId = int.Parse(userIdValue);

           //var recruiterId = await _identityService.GetRecruiterId(userId);

           // if (recruiterId == null)
           //     return BadRequest("Recruiter profile not found.");

            var result = await _mediator.Send(
                new CloseJobCommand
                {
                    id = id,
                   // recruiterId = recruiterId.Value
                });

            if (!result)
                return BadRequest("Job cannot be closed.");

            return Ok("Job closed successfully.");
        }
    }

}
