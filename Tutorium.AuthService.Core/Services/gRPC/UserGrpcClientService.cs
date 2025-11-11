using Grpc.Net.Client;
using Tutorium.Shared;

namespace Tutorium.AuthService.Core.Services
{
    public class UserGrpcClientService
    {
        private readonly User.UserClient _client;

        public UserGrpcClientService(string baseAddress)
        {
            var httpHandler = new HttpClientHandler();
            var channel = GrpcChannel.ForAddress(baseAddress);

            _client = new User.UserClient(channel);
        }

        public async Task<GetOrCreateUserResponse> GetOrCreateUser(string email, string password)
        {
            var request = new GetOrCreateUserRequest { Email = email, Password = password };
            return await _client.GetOrCreateUserAsync(request);
        }
    }
}