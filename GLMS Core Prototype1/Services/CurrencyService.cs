using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace GLMS_Core_Prototype.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ExchangeRateApi:ApiKey"] ?? string.Empty;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Exchange rate API key is not configured.");

            var url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/USD";
            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            return json.RootElement.GetProperty("conversion_rates").GetProperty("ZAR").GetDecimal();
        }
    }
}