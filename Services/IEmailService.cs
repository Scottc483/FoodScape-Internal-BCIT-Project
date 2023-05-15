using Food_Scape.Models;
using SendGrid;
using System.Threading.Tasks;

namespace Food_Scape.Services
{
   
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }
    
}
