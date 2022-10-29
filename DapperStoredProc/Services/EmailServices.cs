//using DapperStoredProc.Models;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace DapperStoredProc.Services
//{
//    public class EmailServices : IEmailServices
//    {
//        string fromMail = "mw7637@outlook.com";
//        string fromMailPassword = "Mianwakaas7637";
//        private readonly EmailCredential _option;
//        public EmailServices(IOptions<EmailCredential> option)
//        {
//            _option = option.Value;
//        }
//        public bool sendEmail(string email, string token)
//        {
//            if (email == null)
//            {
//                return false;
//            }
//            else
//            {
//                var Token = token;
//                var ToEmail = email;
//                MailMessage message = new MailMessage(new MailAddress(fromMail, "DapperProcSP"), new MailAddress(ToEmail));
//                message.Subject = "EmailConfirmation";
//                message.Body = "OTP :" + Token + "thanks";
//                message.IsBodyHtml = true;
//                SmtpClient smtp = new SmtpClient();
//                smtp.Host = "smtp-mail.outlook.com";
//                smtp.Port = 587;
//                smtp.EnableSsl = true;
//                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
//                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
//                credential.UserName = fromMail;
//                credential.Password = fromMailPassword;
//                smtp.UseDefaultCredentials = false;
//                smtp.Credentials = credential;
//                smtp.Send(message);
//                return true;
//            }

//        }
//    }
//}
