using Tutorium.AuthService.Core.Abstractions;
using Tutorium.Grpc.Notification;
using Tutorium.Shared.Utils.Grpc;
using static Tutorium.Grpc.Notification.NotificationGrpc;

namespace Tutorium.AuthService.Grpc.Clients
{
    public class NotificationGrpcSafeClient : BaseGrpcSafeClient<NotificationGrpcClient>, INotificationGrpcClient
    {
        public NotificationGrpcSafeClient(NotificationGrpcClient notificationGrpcClient) : base(notificationGrpcClient) { }

        public Task SendEmailVerificationCodeAsync(string toEmail, string code)
        {
            var request = new SendEmailVerificationCodeRequest() { ToEmail = toEmail, Code = code };

            return ExecuteAsync(
                async () => await _client.SendEmailVerificationCodeAsync(request).ResponseAsync,
                "UserService.CreateUser failed"
            );
        }
    }
}
