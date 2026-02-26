namespace Tutorium.AuthService.Core.Abstractions
{
    public interface IUserGrpcClient
    {
        Task<bool> IsUserExistsAsync(string email);
        Task<int> CreateUserAsync(string email, string passwordHash, DateTime createdAtUtc);
    }
}
