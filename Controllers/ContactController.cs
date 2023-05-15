using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System.Threading.Tasks;



namespace Food_Scape.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Send(IFormCollection collection)
            {
            var apiKey = _configuration["SendGrid_Send:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration["SendGrid_Send:AdminEmail"], $"Support Form");
            var subject = $"User Request {collection["Name"]}";
            var to = new EmailAddress(_configuration["SendGrid_Send:AdminEmail"], "Admin");
            var plainTextContent = $"Name: {collection["Name"]}<br>Email: {collection["Email"]}<br>Message: {collection["Message"]}"; ;
            var htmlContent = $"<strong>{plainTextContent}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return Redirect("/FAQ?success=true");
            }
            else
            {
                return Redirect("/FAQ?success=false&error=Error+sending+email");
            }
        }
    }
}
