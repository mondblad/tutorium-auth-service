using Microsoft.AspNetCore.Identity.Data;
using Tutorium.AuthService.Core.Shared.ValueObjects;

namespace Tutorium.AuthService.Application.Identity.Abstractions.UseCases
{
    public interface ILoginUserUseCase
    {
        Task<string> AuthenticateAsync(Email emial, string password);
    }
}
