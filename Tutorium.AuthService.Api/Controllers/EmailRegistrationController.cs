using Microsoft.AspNetCore.Mvc;
using Tutorium.AuthService.Core.Registration.Abstractions;
using Tutorium.AuthService.Core.Registration.Models;
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

        [HttpPost(template: "/registration")]
        public async Task<ActionResult<Ulid>> GetRegistrationToken()
        {
            return await _registerUseCase.StartRegistration();
        }

        [HttpPut(template: "/registration")]
        public async Task<ActionResult> UpdateValue([FromBody] RegistrationAttemptDto dto)
        {
            await _registerUseCase.UpdateRegistrationAttempt(dto);

            return Ok();
        }

        [HttpPut(template: "/registration/{token}")]
        public async Task<ActionResult<RegistrationAttemptDto>> UpdateValue([FromQuery] Ulid token)
        {
            await _registerUseCase.GetRegistrationAttempt(token);

            return Ok();
        }

        [HttpPost(template: "/registration")]
        public async Task<ActionResult<Ulid>> StartRegistration([FromBody] RegisterDto register)
        {
            return await _registerUseCase.StartRegistration(register.Email, register.Password);
        }

        [HttpPost(template: "/registration/confirm")]
        public async Task<ActionResult> ConfirmRegistration([FromBody] testConfirm conf)
        {
            await _registerUseCase.ConfirmRegistration(conf.Token, conf.Code);

            return Ok();
        }

        public class RegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class testConfirm
        {
            public Ulid Token { get; set; }
            public string Code { get; set; }
        }
    }
}
