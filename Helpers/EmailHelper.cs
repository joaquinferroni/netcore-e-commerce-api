using System.Net;
using System.Net.Mail;
using e_commerce.webapi.Models;
using Microsoft.Extensions.Configuration;
namespace e_commerce.webapi.Helpers
{
    public static class EmailHelper
    {

        internal class EmailConf{
            public string UserName{get;set;}
            public string Password{get;set;}
            public string Host{get;set;}
            public int Port{get;set;}
            public bool SSL{get;set;}
            public bool SendEmail{get;set;}
            public string EmailTest{get;set;}
            public string EmailCCTest{get;set;}
        };


        public static void Send(IConfiguration configuration, IEmailBase email )
        {
            try
            {
                var emailConf = configuration.GetSection("Email").Get<EmailConf>();
                if(!emailConf.SendEmail )
                {
                    email.ToEmail = emailConf.EmailTest;
                    email.CCEmail = emailConf.EmailCCTest;
                }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = emailConf.Host;
                    smtp.Port = emailConf.Port;
                    smtp.EnableSsl = emailConf.SSL;
                    smtp.Credentials = new NetworkCredential(emailConf.UserName, emailConf.Password);

                    using (var message = new MailMessage(emailConf.UserName, email.ToEmail))
                    {
                        message.Subject = email.Subject;
                        message.Bcc.Add(email.CCEmail);
                        message.Body = email.Body;
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
         
    }
}