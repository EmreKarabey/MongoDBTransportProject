using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.GoogleLogin;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.GoogleLogin
{
    public class GoogleLoginService : IGoogleLoginService
    {
        private readonly IConfiguration _config;

        public GoogleLoginService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GoogleJsonWebSignature.Payload> GoogleLoginAsync(string Token)
        {
            GoogleJsonWebSignature.Payload payload;
            var ClientID = _config["Google:ClientID"] ?? throw new InvalidOperationException("Google:ClientID ayarý `appsettings.json` dosyasýnda bulunamadý.");
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { ClientID }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(Token, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new Exception("Geçersiz Google Token!");
            }
        }
    }
}
