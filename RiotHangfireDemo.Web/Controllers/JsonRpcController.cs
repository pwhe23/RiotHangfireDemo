using System;
using System.Linq;
using System.Web.Mvc;
using MediatR;
using Newtonsoft.Json;

namespace RiotHangfireDemo
{
    public class JsonRpcController : Controller
    {
        private readonly IMediator _mediator;

        public JsonRpcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("jsonrpc/{commandName}")]
        public ActionResult Post(string commandName)
        {
            var commandType = Type.GetType($"RiotHangfireDemo.{commandName}", true, true); //HACK:
            var commandJson = Request.InputStream.ReadToEnd();

            return ExecuteCommand(commandType, commandJson);
        }

        [HttpGet, Route("jsonrpc/{commandName}")]
        public ActionResult Get(string commandName)
        {
            var queryString = Request.QueryString.AllKeys.ToDictionary(x => x, x => Request.QueryString[x]);
            var commandType = Type.GetType($"RiotHangfireDemo.{commandName}", true, true); //HACK:
            var commandJson = JsonConvert.SerializeObject(queryString);

            return ExecuteCommand(commandType, commandJson);
        }

        private ActionResult ExecuteCommand(Type type, string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                json = "{}";
            }

            var cmd = JsonConvert.DeserializeObject(json, type);
            var result = _mediator.Execute(cmd);

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }
    };
}