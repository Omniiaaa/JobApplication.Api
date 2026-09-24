using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JobApplication.Application.Interfaces;

namespace JobApplication.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        public Task NotifyCandidate(int applicationId)
        {
            Console.WriteLine(
                $"Notification sent to candidate for application {applicationId}");

            return Task.CompletedTask;
        }

        public Task NotifyRecruiter(int applicationId)
        {
            Console.WriteLine(
                $"Notification sent to recruiter for application {applicationId}");

            return Task.CompletedTask;
        }
    }
}