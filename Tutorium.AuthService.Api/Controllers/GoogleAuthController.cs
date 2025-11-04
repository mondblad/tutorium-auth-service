using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Tutorium.AuthService.Core.Services.Interfaces;

namespace Tutorium.AuthService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleAuthController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpGet("link")]
        public ActionResult<string> GetGoogleAuthLink()
        {
            var redirectUri = GetCallbackUrl();
            var url = _googleAuthService.BuildAuthUrl(redirectUri);
            return Ok(url);
        }

        [HttpGet("callback")]
        public async Task<ActionResult> HandleCallback([FromQuery] string code)
        {
            var redirectUri = GetCallbackUrl();

            var tokens = await _googleAuthService.ExchangeCodeAsync(code, redirectUri);
            var (email, name) = _googleAuthService.ParseIdToken(tokens.IdToken);

            // здесь позже можно добавить логику с Kafka и UserService
            return Ok(new { email, name, tokens });
        }

        private string? GetCallbackUrl()
        {
            return Url.Action(new UrlActionContext()
            {
                Action = nameof(HandleCallback),
                Controller = nameof(GoogleAuthController).Replace("Controller", ""),
                Protocol = Request.Scheme,
                Values = null,
            });
        }
    }
}
