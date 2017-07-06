using System.Web;
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
                authenticationManager.SignOut();

                return CommandResponse.Success();
            }
        };
    };
}