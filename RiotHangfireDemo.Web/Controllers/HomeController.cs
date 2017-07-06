using System.Web.Mvc;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Just used for routing to the Index view which loads Riot.js.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICommander _commander;

        public HomeController(ICommander commander)
        {
            _commander = commander;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(LoginUser model)
        {
            if (model.Email != null)
            {
                var response = _commander.Send(model);
                if (response.IsSuccess)
                {
                    var returnUrl = Request.QueryString["ReturnUrl"] ?? "/";
                    return Redirect(returnUrl);
                }
                response.AddToViewData(ViewData);
            }

            return View(model);
        }

        public ActionResult Logout(LogoutUser model)
        {
            _commander.Send(model);
            return Redirect("/");
        }
    };
}