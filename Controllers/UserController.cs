using System.Threading.Tasks;
using dotnet_new_angular.HelseId;
using Fhi.HelseId.Common.Identity;
using Fhi.HelseId.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace dotnet_new_angular.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly DemoHelseIdConfig _helseIdConfig;
        private readonly RedirectPagesKonfigurasjon _redirectConfig;

        public UserController(IOptions<DemoHelseIdConfig> helseIdOptions, IOptions<RedirectPagesKonfigurasjon> redirectOptions)
        {
            _helseIdConfig = helseIdOptions.Value;
            _redirectConfig = redirectOptions.Value;
        }

        /// <summary>
        /// Taken from AccountController in Fhi.Msis.Klinikermelding.Web.Api.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Logout")]
        [AllowAnonymous]
        public async Task Logout()
        {
            if (_helseIdConfig.AuthUse)
            {
                await HttpContext.SignOutAsync(HelseIdContext.Scheme, new AuthenticationProperties
                {
                    RedirectUri = _redirectConfig.LoggedOut,
                });
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}