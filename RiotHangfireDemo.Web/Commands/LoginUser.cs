using System.Linq;
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
            private readonly IDb _db;

            public Handler(IDb db)
            {
                _db = db;
            }

            public override CommandResponse Handle(LoginUser cmd)
            {
                var user = _db
                    .Query<User>()
                    .SingleOrDefault(x => x.Email == cmd.Email);

                if (user != null && ValidatePassword(cmd.Password, user.Password))
                {
                    SetClaims(user, cmd.RememberMe);

                    return CommandResponse.Success();
                }
                else
                {
                    return CommandResponse.Error("Invalid login");
                }
            }

            private static bool ValidatePassword(string password, string correctHash)
            {
                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(correctHash))
                    return false;

                return PasswordHash.ValidatePassword(password, correctHash);
            }

            private static void SetClaims(User user, bool rememberMe)
            {
                var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                };

                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignIn(authenticationProperties, identity);
            }
        };
    };
}
