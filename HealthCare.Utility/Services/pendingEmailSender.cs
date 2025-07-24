using HealthCareData.Identity;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class PendingEmailSender 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PendingEmailSender> _logger;

        public PendingEmailSender(IServiceProvider serviceProvider, ILogger<PendingEmailSender> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task SendPendingEmailsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var schedulerRepo = scope.ServiceProvider.GetRequiredService<IShedularRepository>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var pendingSchedulers = await schedulerRepo.GetAllAsync("Disease.ApplicationUser");

            foreach (var scheduler in pendingSchedulers.Where(s => !s.IsEmailSent))
            {
                var toEmail = scheduler.Disease?.ApplicationUser?.Email;
                if (string.IsNullOrWhiteSpace(toEmail)) continue;

                var subject = $"Appointment Scheduled - Case #{scheduler.CaseId}";
                var body = $"Dear {scheduler.Disease?.ApplicationUser?.UserName},\n\n" +
                           $"Your appointment is scheduled from {scheduler.dateFrom} to {scheduler.dateTo}.\n\nThank you.";

                await emailSender.SendEmailAsync(toEmail, subject, body);
                await schedulerRepo.MarkEmailAsSentAsync(scheduler.schedulerId);

                _logger.LogInformation($"✅ Email sent for Scheduler ID: {scheduler.schedulerId}");
            }
        }
    }
}
