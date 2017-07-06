using System.Web.Mvc;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public static class HtmlHelpers
    {
        public static void AddToViewData(this CommandResponse commandResponse, ViewDataDictionary viewData)
        {
            viewData["_CommandResponse"] = commandResponse;
        }
    };
}