using System.Threading.Tasks;

namespace WebApplication1.HangfireInterfacesAndRepositories
{
    public interface IEmailService
    {
        public void sendEmail(string email);
        Task SendEmailAsync(string email);
    }
}
