using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.HangfireInterfacesAndRepositories
{
    public class EmailService : IEmailService
    {
        public void sendEmail(string email)
        {
            Console.Write(email);
        }

        private readonly string _smtpServer = "smtp.asimplify.com";
        private readonly int _smtpPort = 465;
        private readonly string _smtpUser = "m@asimplify.com";
        private readonly string _smtpPass = "mmmm@mail1";

        public async Task SendEmailAsync(string email)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mussadiq@asimplify.com"), // Replace with your "from" address
                Subject = "Login Notification",
                Body = "You are logged in",
                IsBodyHtml = false // Set to true if you want to send HTML email
            };
            mailMessage.To.Add(email);

            using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
