using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Services.Helsinki;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Helsinki
{
    public class HelsinkiService : IHelsinkiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public HelsinkiService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<string> Translate(string Comment)
        {
            var apiKey = _config["Helsinki:Token"] ?? throw new InvalidOperationException("Helsinki:Token ayarı `appsettings.json` dosyasında bulunamadı.");
            var url = _config["Helsinki:URL"] ?? throw new InvalidOperationException("Helsinki:URL ayarı `appsettings.json` dosyasında bulunamadı.");

            try
            {
                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    inputs = Comment
                };

                var json = JsonSerializer.Serialize(requestBody);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, stringContent);

                if (!response.IsSuccessStatusCode) return null;

                var file = await response.Content.ReadAsStringAsync();

                var translateComment = "";

                if (file.TrimStart().StartsWith("["))
                {
                    var doc = JsonDocument.Parse(file);

                    translateComment = doc.RootElement[0].GetProperty("translation_text").GetString();

                    return translateComment;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}