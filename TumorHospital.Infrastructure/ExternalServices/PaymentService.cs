using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Cryptography;
using TumorHospital.Application.DTOs.Request.Payment;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Infrastructure.Settings;

namespace TumorHospital.Infrastructure.ExternalServices
{
    public class PaymentService : IPaymentService
    {
        private readonly FawaterakSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentService(IOptions<FawaterakSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> CreateEInvoiceAsync(EInvoiceRequestModel model)
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_settings.BaseUrl}/createInvoiceLink");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.Key);

            var json = JsonConvert.SerializeObject(model);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Fawaterak Error: {content}");

            var result = JsonConvert.DeserializeObject<EInvoiceResponseModel>(content);

            return result?.Data;
        }

        public bool VerifyWebhook(WebHookModel model)
        {
            var query = $"InvoiceId={model.InvoiceId}&InvoiceKey={model.InvoiceKey}&PaymentMethod={model.PaymentMethod}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.Key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(query));

            var generated = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return generated.Equals(
                model.HashKey,
                StringComparison.OrdinalIgnoreCase);
            //return generated == model.HashKey;
        }
    }
}