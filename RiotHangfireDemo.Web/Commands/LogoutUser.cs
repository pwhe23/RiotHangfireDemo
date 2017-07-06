using System.Web;
using Microsoft.AspNet.Identity;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class LogoutUser : Command
    {
        internal class Handler : CommandHandler<LogoutUser, CommandResponse>
        {
            public override CommandResponse Handle(LogoutUser cmd)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return CommandResponse.Success();
            }
        };
    };
}