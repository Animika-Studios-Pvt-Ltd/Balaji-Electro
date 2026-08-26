using System.Web.Mvc;

namespace AudioPlanet.Areas.Enquiry
{
    public class EnquiryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Enquiry"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Enquiry_default",
                "Reports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                //new {controller = "Enquiry", action = "Create", id = UrlParameter.Optional}
                );
        }
    }
}