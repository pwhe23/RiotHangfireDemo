using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// For simplification purposes this is not to the JsonRpc spec.
    /// </summary>
    [Authorize]
    public class JsonRpcController : Controller
    {
        private readonly ICommander _commander;

        public JsonRpcController(ICommander commander)
        {
            _commander = commander;
        }

        /// <summary>
        /// It expects
        /// The Post expects the command class name and json body which will be bound
        /// to the command object.
        /// </summary>
        [HttpPost, Route("jsonrpc/{commandName}")]
        public ActionResult Post(string commandName)
        {
            var commandJson = Request.InputStream.ReadToEnd();

            var result = _commander.Execute(commandName, commandJson);

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }

        /// <summary>
        /// Primarily for testing, we also allow a Get request with the command Name
        /// and QueryString parameters will be bound to the command.
        /// ex: /jsonrpc/DeleteQueueItem?Id=23
        /// </summary>
        [HttpGet, Route("jsonrpc/{commandName}")]
        public ActionResult Get(string commandName)
        {
            var queryString = Request.QueryString.AllKeys.ToDictionary(x => x, x => Request.QueryString[x]);
            var commandJson = JsonConvert.SerializeObject(queryString);

            var result = _commander.Execute(commandName, commandJson);

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }
    };
}