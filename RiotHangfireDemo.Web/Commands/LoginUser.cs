using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class LoginUser : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        internal class Handler : CommandHandler<LoginUser, CommandResponse>
        {
            public override CommandResponse Handle(LoginUser cmd)
            {
                if (cmd.Email.StartsWith("paul@") && !string.IsNullOrWhiteSpace(cmd.Password))
                {
                    var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, cmd.Email));

                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = cmd.RememberMe,
                    };

                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    authenticationManager.SignIn(authenticationProperties, identity);

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
