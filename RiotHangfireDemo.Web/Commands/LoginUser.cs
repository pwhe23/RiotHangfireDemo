using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class LoginUser : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }

        internal class Handler : CommandHandler<LoginUser, CommandResponse>
        {
            public override CommandResponse Handle(LoginUser cmd)
            {
                if (cmd.Email.StartsWith("paul@") && !string.IsNullOrWhiteSpace(cmd.Password))
                {
                    var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, cmd.Email));

                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    authenticationManager.SignIn(identity);

                    return CommandResponse.Success();
                }
                else
                {
                    return CommandResponse.Error("Invalid login");
                }
            }
        };
    };
}
