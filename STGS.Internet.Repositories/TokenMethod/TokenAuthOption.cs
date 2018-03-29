using Microsoft.IdentityModel.Tokens;
using System;

namespace STGS.Internet.Repositories.TokenMethod
{
    public class TokenAuthOption
    {
        public SigningCredentials SigningKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore => DateTime.Now;

        /// <summary>
        /// Token有效时间
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(7);

        public DateTime IssuedAt => DateTime.Now;

        public DateTime Expiration => IssuedAt.Add(ValidFor);

    }
}
