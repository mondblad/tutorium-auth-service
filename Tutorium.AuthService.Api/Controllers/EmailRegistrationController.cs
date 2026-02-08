using Microsoft.AspNetCore.Mvc;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models.RegistrationDraft;
using Tutorium.Shared.Utils.Controllers;

namespace Tutorium.AuthService.Api.Controllers
{
    public class EmailRegistrationController : BaseController
    {
        private readonly IRegisterUseCase _registerUseCase;

        public EmailRegistrationController(IRegisterUseCase registerUseCase) 
        {
            _registerUseCase = registerUseCase;
        }

        [HttpGet(template: "/registration/drafts/{draftToken}")]
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
        }
    }
}
