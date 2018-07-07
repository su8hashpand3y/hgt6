using System.Threading.Tasks;

namespace HGT6.Helpers
{
    public interface IEmailSender
    {
        bool SendMail(string email, string subject, string message);
    }
}