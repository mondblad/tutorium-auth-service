using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Tutorium.AuthService.Core.Services;
using Tutorium.AuthService.Core.Services.Interfaces;

namespace Tutorium.AuthService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserGrpcClientService _userGrpcClientService;

        public GoogleAuthController(
            IGoogleAuthService googleAuthService,
            IJwtTokenService jwtTokenService,
            UserGrpcClientService userGrpcClientService)
        {
            _googleAuthService = googleAuthService;
            _userGrpcClientService = userGrpcClientService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("link")]
        public ActionResult<string> GetGoogleAuthLink() => _googleAuthService.BuildAuthUrl(GetCallbackUrl());

        [HttpGet("callback")]
        public async Task<ActionResult> HandleCallback([FromQuery] string code)
        {
            var tokens = await _googleAuthService.ExchangeCodeAsync(code, GetCallbackUrl());
            var (email, name) = _googleAuthService.ParseIdToken(tokens.IdToken);

            if (email is null)
                return Forbid();

            var user = await _userGrpcClientService.GetOrCreateUser(email, null);
            if (user is null)
                return NotFound();

            var jwt = _jwtTokenService.GenerateToken(user.UserId, email);

            var redirectUrl = $"https://localhost:3000/oauth/callback?token={jwt}";

            return Redirect(redirectUrl);
        }

        private string GetCallbackUrl()
        {
            var url = Url.Action(
                new UrlActionContext()
                {
                    Action = nameof(HandleCallback),
                    Controller = nameof(GoogleAuthController).Replace("Controller", ""),
                    Protocol = Request.Scheme,
                    Values = null,
                }
            );

            if (string.IsNullOrEmpty(url))
                throw new InvalidOperationException("Не удалось сгенерировать callback URL");

            return url;
        }
    }
}
