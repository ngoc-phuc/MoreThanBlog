namespace Core.Utils
{
    public class JwtTokenResultModel
    {
        public string UserId { get; set; }

        public string TokenType { get; set; }

        public string AccessToken { get; set; }

        public double ExpiresInSeconds { get; set; }
    }
}