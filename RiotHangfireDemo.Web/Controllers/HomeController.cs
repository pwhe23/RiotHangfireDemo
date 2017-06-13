using System.Web.Mvc;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Just used for routing to the Index view which loads Riot.js.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    };
}