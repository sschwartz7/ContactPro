using Microsoft.AspNetCore.Identity.UI.Services;
using System.Drawing.Text;
using ContactPro.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ContactPro.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {


            try
            {
                var emailAddress = _emailSettings.EmailAddress ??
                    Environment.GetEnvironmentVariable("EmailAddress");
                var emailPassword = _emailSettings.EmailPassword ??
                    Environment.GetEnvironmentVariable("EmailPassword");
                var emailHost = _emailSettings.EmailHost ??
                    Environment.GetEnvironmentVariable("EmailHost");
                var emailPort = _emailSettings.EmailPort != 0 ? _emailSettings.EmailPort
                    : int.Parse(Environment.GetEnvironmentVariable("EmailPort")!);

                MimeMessage newEmail = new MimeMessage();

                //attach the email sender
                newEmail.Sender = MailboxAddress.Parse(emailAddress);

                //attach the email recipient
                foreach (string address in email.Split(";"))
                {
                    newEmail.Bcc.Add(MailboxAddress.Parse(address));
                }

                //Set the Subject
                newEmail.Subject = subject;

                //format the body
                BodyBuilder emailBody = new BodyBuilder();
                emailBody.HtmlBody = htmlMessage;
                newEmail.Body = emailBody.ToMessageBody();

                //Prep the service and send the email
                using SmtpClient smtpClient = new SmtpClient();//use the mailkit Smtp client

                try
                {
//                    ConnectAsync() = “go to gmail.com”

//                    AuthenticateAsync() = “go log in at gmail.com”

//                    SendEmailAsync() = “go send the email we just made”

//                    DisconnectAsync() = “go log out”
                    await smtpClient.ConnectAsync(emailHost, emailPort, SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(emailAddress, emailPassword);
                    await smtpClient.SendAsync(newEmail);

                    await smtpClient.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    //var error = ex.Message;
                    throw;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
