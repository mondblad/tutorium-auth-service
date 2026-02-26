namespace Tutorium.AuthService.Core.Models.JwtToken
{
    public class JwtTokenOptions
    {
        public string Secret { get; set; }
        public string FrontendUrl { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
