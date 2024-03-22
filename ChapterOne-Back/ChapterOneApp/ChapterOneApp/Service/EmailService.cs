
using System;
using Microsoft.Extensions.Options;
using ChapterOneApp.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ChapterOneApp.Service.Interfaces;



namespace ChapterOneApp.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        public EmailService(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }



        public void Send(string to, string subject, string html, string from = null)
        {
            // create message

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailSetting.FromAddress));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Server, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.UserName, _emailSetting.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
