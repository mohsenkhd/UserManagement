using Application.ServiceContracts.Notifier;
using Application.ViewModels.Notifier;
using Common.Exceptions.UserManagement;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;


namespace Service.Notifier
{
    public class NotifierService : INotifierService
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _configuration;

        public NotifierService(IConfiguration configuration, IHttpClientFactory http)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<SendMessageBaseViewModel> SendMessageAsync(SendMessageCommandModel model)
        {
            var url = _configuration["Url:Notifier"];
            var apiKey = _configuration["NotifierKey:X-API-KEY"];
            var apiSecret = _configuration["NotifierKey:X-API-SECRET"];
            var payload = new
            {
                destination = model.Destination,
                title = model.Title,
                body = model.Body,
                channel = model.Channel,
                messageTypeId = model.MessageTypeId
            };

            using var httpClient = _http.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            httpClient.DefaultRequestHeaders.Add("X-API-SECRET", apiSecret);

            var payloadJson = JsonConvert.SerializeObject(payload);
            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            var httpResponseMessage = await httpClient.PostAsync(url, content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                using var streamReader = new System.IO.StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                var serializer = new Newtonsoft.Json.JsonSerializer();
                var response = serializer.Deserialize<SendMessageBaseViewModel>(jsonReader);

                if (response != null)
                {
                    return response;
                }
                else
                {
                    throw UserManagementExceptions.SendMessageFaildException;
                }
            }
            else
            {
                throw UserManagementExceptions.SendMessageFaildException;
            }
        }
    }
}
