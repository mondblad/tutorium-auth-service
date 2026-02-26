using Tutorium.Grpc.User;
using static Tutorium.Grpc.User.UserGrpc;
using Tutorium.Shared.Utils.Grpc;
using Tutorium.AuthService.Core.Abstractions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Tutorium.AuthService.Grpc.Clients
{
    public class UserGrpcSafeClient : BaseGrpcSafeClient<UserGrpcClient>, IUserGrpcClient
    {
        public UserGrpcSafeClient(UserGrpcClient client) : base(client) { }
        
        public Task<bool> IsUserExistsAsync(string email)
        {
            var request = new IsUserExistsRequest() { Email = email };

            return ExecuteAsync(
                async () =>
                {
                    var response = await _client.IsUserExistsAsync(request);
                    return response.Exists;
                },
                "UserService.IsUserExists failed");
        }

        public Task<int> CreateUserAsync(string email, string passwordHash, DateTime createdAtUtc)
        {
            var request = new CreateUserRequest()
            {
                Email = email,
                PasswordHash = passwordHash,
                CreatedAtUtc = createdAtUtc.ToTimestamp(),
            };

            return ExecuteAsync(
                async () =>
                {
                    var response = await _client.CreateUserAsync(request).ResponseAsync;
                    return response.UserId;
                },
                "UserService.CreateUser failed"
            );
        }
    }
}
