using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MatchMaker.Api.AppSettings;
using MatchMaker.Api.Services.Jwt;
using MatchMaker.Shared.Common;
using Microsoft.Extensions.Options;

namespace MatchMaker.Api.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<AccountSettings> _accountSettings;
        private readonly IJwtEncoder _encoder;
        private readonly IJwtDecoder _decoder;

        public JwtService(IOptions<AccountSettings> accountSettings)
        {
            Guard.NotNull(accountSettings, nameof(accountSettings));

            this._accountSettings = accountSettings;

            var algorithm = new HMACSHA256Algorithm();
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var provider = new UtcDateTimeProvider();
            var validator = new JwtValidator(serializer, provider);

            this._encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            this._decoder = new JwtDecoder(serializer, validator, urlEncoder);
        }

        public string Create(int accountId, string emailAddress)
        {
            Guard.NotZeroOrNegative(accountId, nameof(accountId));
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

            var payload = new Dictionary<string, object>
            {
                [JwtClaims.AccountId] = accountId,
                [JwtClaims.EmailAddress] = emailAddress
            };

            return this._encoder.Encode(payload, this._accountSettings.Value.JwtSecret);
        }

        public bool Validate(string token)
        {
            Guard.NotNullOrWhiteSpace(token, nameof(token));

            try
            {
                this._decoder.Decode(token, this._accountSettings.Value.JwtSecret, verify: true);

                return true;
            }
            catch (TokenExpiredException)
            {
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
        }
    }
}