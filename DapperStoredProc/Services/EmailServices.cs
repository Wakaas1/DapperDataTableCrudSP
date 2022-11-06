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

        public void SendEmail(string email, string token, string name)
        {


            string Link = token;



            var emailCredential = _configuration.GetSection("EmailCredentials");

            var data = emailCredential.Get<EmailCredential>();

            var path = _webHost.ContentRootPath + Path.DirectorySeparatorChar.ToString()
                + "Email" + Path.DirectorySeparatorChar.ToString() + "EmailTemp.html";
            string pathToDb = @"wwwroot\Email\EmailTemp.html";



            string mailHtmlbody = "";

            using (StreamReader steamReader = File.OpenText(pathToDb))
            {
                mailHtmlbody = steamReader.ReadToEnd();
            }


            mailHtmlbody= mailHtmlbody.Replace("#:HrefLink:#", Link);

            mailHtmlbody = mailHtmlbody.Replace("#:Name:#", name);
            //string mailBody = string.Format(mailHtmlbody, name, Link);

          
                MailMessage message = new MailMessage(new MailAddress(data.UserName, "DapperProcSP"), new MailAddress(email));
                message.Subject = "EmailConfirmation";
                message.Body = mailHtmlbody;
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
               
            

        }
    }
}
