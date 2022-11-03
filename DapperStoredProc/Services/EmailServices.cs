using DapperStoredProc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHost;

        public EmailServices(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHost = webHostEnvironment;
        }

        public bool sendEmail(string email, string token)
        {
            var emailTemp = _webHost.ContentRootPath + Path.DirectorySeparatorChar.ToString()
                + "EmailTemp" + Path.DirectorySeparatorChar.ToString() + "EmailTemp.html";

            var cred = _configuration.GetSection("EmailCredentials");
            var data = cred.Get<EmailCredential>();
            if (email == null)
            {
                return false;
            }
            else
            {
                var Token = token;
                var ToEmail = email;
                MailMessage message = new MailMessage(new MailAddress(data.UserName, "DapperProcSP"), new MailAddress(ToEmail));
                message.Subject = "EmailConfirmation";
                message.Body = "OTP :" + Token + "thanks";
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
                credential.UserName = data.UserName;
                credential.Password = data.Password;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Send(message);
                return true;
            }

        }
    }
}
