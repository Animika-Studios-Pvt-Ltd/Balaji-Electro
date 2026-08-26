using ActionMailer.Net.Mvc;
using AudioPlanet.Areas.Enquiry.Models;
using AudioPlanet.Models;
using System;
using System.Configuration;

namespace AudioPlanet.Controllers
{
    public class MailController : MailerBase
    {
        //public EmailResult ReplyToEnquiry(EnquiryReply obj)
        //{
        //    base.get_To().Add(obj.enquiry.Email);
        //    base.set_From(ConfigurationManager.AppSettings["FromEmail"]);
        //    base.set_Subject("Re: Enquiry on audioplanet.co.in");
        //    return this.Email("ReplyToEnquiry", obj, null, true);
        //}

        public EmailResult NewEnquiry(Enquiry obj)
        {
            To.Add("india.lumos@gmail.com");
            //To.Add("audioplanetblr@gmail.com");
            //BCC.Add("sunilr33@gmail.com");
            From = ConfigurationManager.AppSettings["FromEmail"];
            Subject = "[Audio Planet] - New Business Enquiry";
            return Email("NewEnquiry", obj, null, true);
        }

        //public EmailResult PasswordReset(AdminUser obj)
        //{
        //    base.get_To().Add(obj.Email);
        //    base.get_BCC().Add("sshreekumar9@gmail.com");
        //    base.set_From(ConfigurationManager.AppSettings["FromEmail"]);
        //    base.set_Subject("Audio Planet (Admin area) - Account Recovery");
        //    return this.Email("PasswordReset", obj, null, true);
        //}

        public MailController() : base(null, null)
        {
        }
    }
}
