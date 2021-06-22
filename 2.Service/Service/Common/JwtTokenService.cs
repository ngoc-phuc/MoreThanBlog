using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abstraction.Service.Common;
using Core.Helper;
using Core.Utils;
using Microsoft.Extensions.Options;

namespace Service.Common
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtTokenService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<JwtTokenResultModel> RequestTokenAsync(ClaimsIdentity identity)
        {
            return new JwtTokenResultModel
            {
                UserId = identity.GetUserId(),
                TokenType = "Bearer",
                AccessToken = await GenerateEncodedToken(identity),
                ExpiresInSeconds = _jwtOptions.ValidFor.TotalSeconds
            };
        }

        private async Task<string> GenerateEncodedToken(ClaimsIdentity identity)
        {
            _jwtOptions.IssuedAt = DateTime.Now;

            var claims = identity.Claims.Union(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.LocalToUtcTime().ToSecondsTimestamp().ToString(), ClaimValueTypes.Integer64),
                });

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.IssuedAt,
                GetExpirationDate(),
                _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private DateTime GetExpirationDate()
        {
            return _jwtOptions.IssuedAt.AddHours(_jwtOptions.WebExpirationInHours);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.WebExpirationInHours <= 0)
                throw new ArgumentException(@"Must be a non-zero value.", nameof(JwtIssuerOptions.WebExpirationInHours));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}