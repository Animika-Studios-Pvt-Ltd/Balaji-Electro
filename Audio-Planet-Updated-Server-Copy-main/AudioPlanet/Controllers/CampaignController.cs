using System.Web.Hosting;
using System.Web.Mvc;

namespace AudioPlanet.Controllers
{
    public class CampaignController : Controller
    {
        //
        // GET: /Campaign/

        public ActionResult Index(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return View(url);
            }
            return Redirect("/");
        }
    }
}
