using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace AudioPlanet.Areas.Enquiry.Controllers
{
    public class Helper
    {
        public void SendMail(string toEmailID, string subject, string mailBody, string replyMailID)
        {
            string messageBodyString = "<html><body>" + mailBody + "</body></html>";
            var mail = new MailMessage(ConfigurationManager.AppSettings.Get("EmailAddress"), toEmailID, subject,
                                       messageBodyString);
            if (replyMailID != "")
            {
                mail.ReplyToList.Add(replyMailID);
            }
            mail.IsBodyHtml = true;
            var smtp = new SmtpClient(ConfigurationManager.AppSettings.Get("MailOutgoingPort"),
                                      Convert.ToInt16(ConfigurationManager.AppSettings.Get("SMTP")))
                {
                    UseDefaultCredentials = false,
                    Credentials =
                        new NetworkCredential(ConfigurationManager.AppSettings.Get("EmailAddress"),
                                              ConfigurationManager.AppSettings.Get("Password")),
                    Timeout = 300000
                };
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }
    }
}