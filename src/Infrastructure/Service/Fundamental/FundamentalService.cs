using Application.ServiceContracts.Fundamental;
using Application.ViewModels.Fundamental;
using Common.Exceptions.UserManagement;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;


namespace Service.Fundamental
{
    public class FundamentalService : IFundamentalService
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        public FundamentalService(IConfiguration configuration, IHttpClientFactory http)
        {
            _http = http;
            _configuration = configuration;
            _url = _configuration["Url:Fundamental"];
            _apiKey = _configuration["NotifierKey:X-API-KEY"];
            _apiSecret = _configuration["NotifierKey:X-API-SECRET"];
        }

        public async Task<ShahkarBaseViewModel> Shahkar(ShahkarCommandModel model)
        {
            var payload = new
            {
                nationalCode = model.NationalCode,
                mobileNumber = model.MobileNumber,
            };

            using var httpClient = _http.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            httpClient.DefaultRequestHeaders.Add("X-API-SECRET", _apiSecret);

            var payloadJson = JsonSerializer.Serialize(payload);
            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            var httpResponseMessage = await httpClient.PostAsync(_url + "/services/inquiry/shahkar", content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var response = await JsonSerializer.DeserializeAsync<ShahkarBaseViewModel>(contentStream) ?? throw UserManagementExceptions.ShahkarServiceFaildException;
                return response;
            }
            else
            {
                throw UserManagementExceptions.ShahkarServiceFaildException;
            }
        }

        public async Task<CustomerBaseViewModel> Customer(CustomerCommandModel model)
        {
            var payload = new
            {
                nationalId = model.NationalId,
                mobileNumber = model.MobileNumber,
            };

            using var httpClient = _http.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            httpClient.DefaultRequestHeaders.Add("X-API-SECRET", _apiSecret);

            var payloadJson = JsonSerializer.Serialize(payload);
            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            var httpResponseMessage = await httpClient.PostAsync(_url + "/services/inquiry/customer", content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var response = await JsonSerializer.DeserializeAsync<CustomerBaseViewModel>(contentStream) ?? throw UserManagementExceptions.CustomerServiceFaildException;
                return response;
            }
            else
            {
                throw UserManagementExceptions.CustomerServiceFaildException;
            }
        }

        public async Task<ContentExceptionBaseViewModel> ContentException(ContentExceptionCommandModel model)
        {
            using var httpClient = _http.CreateClient();

            var httpResponseMessage = await httpClient.GetAsync(_url + "/content?key=" + model.Key);
            httpResponseMessage.EnsureSuccessStatusCode();
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();


            return new ContentExceptionBaseViewModel
            {
                ErrorMessage=responseContent
            };
        }
    }
}

