using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class JsonRpcController : Controller
    {
        private readonly ICommander _commander;

        public JsonRpcController(ICommander commander)
        {
            _commander = commander;
        }

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