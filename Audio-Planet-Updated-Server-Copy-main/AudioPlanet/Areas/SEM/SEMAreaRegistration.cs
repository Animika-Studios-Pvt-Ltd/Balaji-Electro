using System.Web.Mvc;

namespace AudioPlanet.Areas.SEM
{
    public class SEMAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SEM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SEM_default",
                "SEM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
