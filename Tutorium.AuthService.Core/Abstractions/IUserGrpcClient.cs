namespace Tutorium.AuthService.Core.Abstractions
{
    public interface IUserGrpcClient
    {
        Task<bool> IsUserExistsAsync(string email);
        Task CreateUserAsync(string email, string passwordHash, DateTime createdAtUtc);
    }
}
