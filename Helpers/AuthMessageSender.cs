using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HGT6.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace HGT6.Helpers
{
    public class AuthMessageSender : IEmailSender
    {
        public bool SendMail(string email, string subject, string message)
        {
            bool sent= false;
            try
            {
                const String FROM = "himachal@himachalgottalent.com";
                const String FROMNAME = "himachal@himachalgottalent.com";

                String TO = email;
                var list  =Environment.GetEnvironmentVariables();
                String SMTP_USERNAME = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                String SMTP_PASSWORD = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
                // If you're using Amazon SES in a region other than US West (Oregon), 
                // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
                // endpoint in the appropriate Region.
                const String HOST = "email-smtp.eu-west-1.amazonaws.com";

                // The port you will connect to on the Amazon SES SMTP endpoint. We
                // are choosing port 587 because we will use STARTTLS to encrypt
                // the connection.
                const int PORT = 587;

                // The subject line of the email
                String SUBJECT = subject;

                // The body of the email
                String BODY = message;
                  

                // Create and build a new MailMessage object
                MailMessage MailMessage = new MailMessage();
                MailMessage.IsBodyHtml = true;
                MailMessage.From = new MailAddress(FROM, FROMNAME);
                MailMessage.To.Add(new MailAddress(TO));
                MailMessage.Subject = SUBJECT;
                MailMessage.Body = BODY;
                // Comment or delete the next line if you are not using a configuration set

                // Create and configure a new SmtpClient
                SmtpClient client =
                    new SmtpClient(HOST, PORT);
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                // Enable SSL encryption
                client.EnableSsl = true;
                client.Send(MailMessage);
                sent = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return sent;
        }
    }
}