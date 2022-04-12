using Microsoft.Extensions.Configuration;
using rolesDemoSSD.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace rolesDemoSSD.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Response> SendSingleEmail(ComposeEmailModel payload)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("huzi.blizzard96@gmail.com", "Team Hamsters");
            var subject = payload.Subject;
            var to = new EmailAddress(payload.Email
            , $"{payload.FirstName} {payload.LastName}");
            var textContent = payload.Body;
            var htmlContent = $"<strong>{payload.Body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject
            , textContent, htmlContent);
            var request = client.SendEmailAsync(msg);
            request.Wait();
            var result = request.Result;
            return request;
        }
    }
}
