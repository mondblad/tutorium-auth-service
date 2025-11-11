using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Tutorium.Shared;
using Tutorium.Shared.Options;

namespace Tutorium.AuthService.Core.Services
{
    public class UserGrpcClientService
    {
        private readonly User.UserClient _client;

        public UserGrpcClientService(IOptions<GrpcSettings> grpcSettings)
        {
            var httpHandler = new HttpClientHandler();
            var channel = GrpcChannel.ForAddress(grpcSettings.Value.UserServiceUrl);

            _client = new User.UserClient(channel);
        }

        public async Task<GetOrCreateUserResponse> GetOrCreateUser(string email, string password)
        {
            var request = new GetOrCreateUserRequest { Email = email, Password = password };
            return await _client.GetOrCreateUserAsync(request);
        }
    }
}