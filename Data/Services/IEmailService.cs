using Day3Paypal.Models;
using SendGrid;
using System.Threading.Tasks;

namespace Day3Paypal.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }
}
