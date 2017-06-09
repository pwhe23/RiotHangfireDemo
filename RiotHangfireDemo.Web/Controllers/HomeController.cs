using System.Web.Mvc;
using MediatR;

namespace RiotHangfireDemo
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddTask()
        {
            _mediator.Send(new AddTask
            {

            });
            return RedirectToAction(nameof(Index));
        }
    };
}