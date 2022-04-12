using rolesDemoSSD.Models;
using SendGrid;
using System.Threading.Tasks;


namespace rolesDemoSSD.Data.Services { 

public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }
}
