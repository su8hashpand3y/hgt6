using System.Threading.Tasks;

namespace HGT6.Helpers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}