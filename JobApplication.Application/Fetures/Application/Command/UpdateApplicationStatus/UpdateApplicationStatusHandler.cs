using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Fetures.Application.Command.UpdateApplicationStatus
{
    public class UpdateApplicationStatusHandler : IRequestHandler<UpdateApplicationStatusCommand, bool>
    {
        private readonly IApplicationRepository _applicationRepository;

        public UpdateApplicationStatusHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<bool> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(request.Id);

            if (application == null)
                return false;

            if (request.NewStatus <= application.JobApplicationStatus)
                return false;

            application.JobApplicationStatus = request.NewStatus;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();

            return true;
        }
    }
}
