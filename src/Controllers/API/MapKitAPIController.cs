using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MapKitJSSample.Helpers;
using MapKitJSSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MapKitJSSample.Controllers.API
{
    public class MapKitAPIController : Controller
    {
        private MapKitSettings _mapKitSettings;

        public MapKitAPIController(IOptions<MapKitSettings> mapKitSettings)
        {
            if (mapKitSettings != null)
                _mapKitSettings = mapKitSettings.Value;
        }

        [Route("api/mapkit/gettoken")]
        public string GetMapKitToken()
        {
            var header = new
            {
                // The encryption algorithm (alg) used to encrypt the token. ES256 should be used to
                // encrypt your token, and the value for this field should be "ES256".
                alg = "ES256",

                // A 10-character key identifier (kid) key, obtained from your Apple Developer account.
                kid = _mapKitSettings.KeyIdentifier,

                // A type parameter (typ), with the value "JWT".
                typ = "JWT"
            };

            var payload = new
            {
                // The Issuer (iss) registered claim key. This key's value is your 10-character Team ID,
                // obtained from your developer account.
                iss = _mapKitSettings.TeamID,

                // The Issued At (iat) registered claim key. The value of this key indicates the time at
                // which the token was generated, in terms of the number of seconds since UNIX Epoch, in UTC.
                iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                // The Expiration Time (exp) registered claim key, in terms of the number of seconds since
                // UNIX Epoch, in UTC.
                exp = DateTimeOffset.UtcNow.AddMinutes(_mapKitSettings.TokenExpirationMinutes).ToUnixTimeSeconds(),

                // This key's value is a fully qualified domain that should match the Origin header passed
                // by a browser.  Apple compares this to your requests for verification.  Note that you can
                // omit this and get warnings, though it's definitely not recommended. 
                origin = _mapKitSettings.Origin
            };

            var headerBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header));
            var payloadBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));

            var message = EncodingHelper.JwtBase64Encode(headerBytes)
                + "." + EncodingHelper.JwtBase64Encode(payloadBytes);

            var messageBytes = Encoding.UTF8.GetBytes(message);

            var crypto = ECDsa.Create();
            crypto.ImportPkcs8PrivateKey(Convert.FromBase64String(_mapKitSettings.PrivateKey), out _);

            var signature = crypto.SignData(messageBytes, HashAlgorithmName.SHA256);

            return message + "." + EncodingHelper.JwtBase64Encode(signature);
        }
    }
}
