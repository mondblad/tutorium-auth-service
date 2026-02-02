namespace Tutorium.AuthService.Core.Abstractions
{
    public interface INotificationGrpcClient
    {
        Task SendEmailVerificationCodeAsync(string toEmail, string code);
    }
}
