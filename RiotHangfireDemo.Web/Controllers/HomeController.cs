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

        public ActionResult Index()
        {
            var tasks = _mediator.Send(new QueryTasks());
            return View(tasks);
        }

        public ActionResult AddTask(AddTask cmd)
        {
            _mediator.Send(cmd);
            return RedirectToAction(nameof(Index));
        }
    };
}