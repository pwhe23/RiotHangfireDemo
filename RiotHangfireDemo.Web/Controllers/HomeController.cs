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

        public ActionResult Index(QueryQueueItems cmd)
        {
            var result = _mediator.Send(cmd);
            return View(result);
        }

        public ActionResult QueueEmail(CreateFakeEmail cmd)
        {
            _mediator.Send(cmd);
            return RedirectToAction(nameof(Index));
        }
    };
}