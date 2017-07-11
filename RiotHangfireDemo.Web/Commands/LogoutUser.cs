using System;
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
                try
                {
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

                    authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    return CommandResponse.Success();
                }
                catch (Exception ex)
                {
                    return CommandResponse.Error(ex);
                }
            }
        };
    };
}