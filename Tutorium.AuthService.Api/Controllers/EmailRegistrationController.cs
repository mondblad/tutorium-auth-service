using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorium.AuthService.Application.Identity.Abstractions.UseCases;
using Tutorium.AuthService.Application.Identity.DTOs;
using Tutorium.AuthService.Application.Identity.ValueObjects;
using Tutorium.AuthService.Core.Sessions.Models;
using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.Shared.Utils.Controllers;

namespace Tutorium.AuthService.Api.Controllers
{
    public class EmailRegistrationController : BaseController
    {
        private readonly IRegistrationUseCase _registrationUseCase;
        private readonly ILoginUserUseCase _loginUserUseCase;

        public EmailRegistrationController(IRegistrationUseCase registrationUseCase, ILoginUserUseCase loginUserUseCase) 
        {
            _registrationUseCase = registrationUseCase;
            _loginUserUseCase = loginUserUseCase;
        }

        [AllowAnonymous]
        [HttpPost(template: "registration/initiateRegistration")]
        public async Task<ActionResult<RegistrationResponse>> PostInitiateRegistrationAsync([FromBody] AuthRequest dto)
        {
            var email = Email.Create(dto.Email);

            var token = await _registrationUseCase.InitiateRegistrationAsync(email, dto.Password);

            return new RegistrationResponse(token.ToString("N"));
        }

        [AllowAnonymous]
        [HttpPost(template: "registration/confirmRegistration/{token}")]
        public async Task<ActionResult> PostConfirmRegistrationAsync(string token, [FromBody] ConfirmRegistrationRequest dto)
        {
            if (!Guid.TryParse(token, out var guid))
                return BadRequest("Invalid token format");

            var verificationCode = VerificationCode.CreateFromString(dto.Code);

            var session = await _registrationUseCase.ConfirmRegistrationAsync(guid, verificationCode);

            SetSessionInCookies(session);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet(template: "registration/email/{token}")]
        public async Task<ActionResult<RegistrationEmailResponse>> GetRegistrationEmailAsync(string token)
        {
            if (!Guid.TryParse(token, out var guid))
                return BadRequest("Invalid token format");

            var email = await _registrationUseCase.GetEmailAsync(guid);

            return new RegistrationEmailResponse(email.Value);
        }

        [AllowAnonymous]
        [HttpPost(template: "login/")]
        public async Task<ActionResult> PostLoginAsync([FromBody] AuthRequest dto)
        {
            var email = Email.Create(dto.Email);

            var session = await _loginUserUseCase.AuthenticateAsync(email, dto.Password);

            SetSessionInCookies(session);

            return Ok();
        }

        private void SetSessionInCookies(Session session)
        {
            Response.Cookies.Append("session_id", session.SessionId, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(7)
            });
        }

        /*[HttpGet(template: "/registration/drafts/{draftToken}")]
        public async Task<ActionResult<RegistrationDraftRuntimeDto>> GetRegistrationDraft(string draftToken)
        {
            return await _registerUseCase.GetRegistrationDraft(Guid.Parse(draftToken));
        }

        [HttpPost(template: "/registration/drafts")]
        public async Task<ActionResult<string>> CreateRegistrationDraft([FromBody] RegistrationDraftRuntimeCreateDto createDto)
        {
            var draftToken = await _registerUseCase.CreateRegistrationDraft(createDto);
            
            return draftToken.ToString("N");
        }

        [HttpPut(template: "/registration/drafts/{draftToken}")]
        public async Task<ActionResult> UpdateRegistrationDraft(string draftToken, [FromBody] RegistrationDraftRuntimeUpdateDto updateDto)
        {
            await _registerUseCase.UpdateRegistrationDraft(Guid.Parse(draftToken), updateDto);

            return Ok();
        }

        [HttpPost(template: "/registration/confirm/{draftToken}")]
        public async Task<ActionResult<string>> SendConfirmationCode(string draftToken, [FromBody] RegistrationDraftRuntimeSubmitDto submitDto)
        {
            var emailConfirmToken = await _registerUseCase.SendConfirmationCode(Guid.Parse(draftToken), submitDto);

            return emailConfirmToken.ToString("N");
        }
        
        [HttpPut(template: "/registration/confirm/{token}")]
        public async Task<ActionResult> ConfirmCode(string token, [FromBody] RegistrationDraftRuntimeSubmitDto submitDto)
        {
            await _registerUseCase.SendConfirmationCode(Guid.Parse(token), submitDto);

            return Ok();
        }

        public class RegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }*/
    }
}
