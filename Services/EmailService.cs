using Food_Scape.Models;
using Food_Scape.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
namespace Food_Scape.Data.Services
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
            var apiKey = _configuration.GetSection("SendGrid")["ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("bcit.foodscape@gmail.com", "FoodScape");
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