using Tutorium.AuthService.Core.Shared.ValueObjects;
using Tutorium.AuthService.Core.Sessions.Models;

namespace Tutorium.AuthService.Application.Identity.Abstractions.UseCases
{
    public interface ILoginUserUseCase
    {
        Task<Session> AuthenticateAsync(Email emial, string password);
    }
}
