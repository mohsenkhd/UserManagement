using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Common.Wrappers
{
    public class CaptchaWrapper
    {
        private const string HttpClient = "HttpClient";
        private object? _req;
        private string? _url;
        private HttpMethod? _method;
        private readonly IHttpClientFactory _http;
        private readonly ILogger<CaptchaWrapper> _logger;

        private readonly Dictionary<string, string> _headers = new();

        public CaptchaWrapper(IHttpClientFactory http, ILogger<CaptchaWrapper> logger)
        {
            _http = http;
            _logger = logger;
        }

        private Dictionary<string, string> Query { get; set; } = new();


        private void CheckForAllParameters()
        {
            var list = new List<object?>() { _url, _method };
            if (list.All(parameter => parameter != null)) return;

            throw new Exception();
        }

        public CaptchaWrapper WithBody(object? req)
        {
            _req = req;
            return this;
        }

        public CaptchaWrapper WithUserId(long id)
        {
            return this;
        }

        public CaptchaWrapper WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public CaptchaWrapper WithHttpMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }


        public async Task<TRes> Fetch<TRes>()
        {
            CheckForAllParameters();

            var message = GetHttpMessage();

            AddHttpRequestHeaders(message);

            if (message.Content != null)
                message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var res = await _http.CreateClient(HttpClient).SendAsync(message);
            Clear();

            var resContent = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    var result = JsonSerializer.Deserialize<TRes>(resContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    if (result == null)
                        throw new Exception("An Error occured Deserializing the json response \n " + resContent);
                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError("An Error occured during deserialization of response : {Res}", resContent);
                    _logger.LogError("Error : {Err}", e);
                    throw;
                }
            }

            _logger.LogError("An error occured during the http call {Res} : ", resContent);

            throw new Exception();
        }


        private void Clear()
        {
            _headers.Clear();
            Query.Clear();
            _method = null;
            _req = null;
        }


        private void AddHttpRequestHeaders(HttpRequestMessage message)
        {
            foreach (var header in _headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }
        }


        private HttpRequestMessage GetHttpMessage()
        {
            return new HttpRequestMessage
            {
                Content = (_method == HttpMethod.Get || _req == null)
                    ? null
                    : new StringContent(GetRequestBody(), Encoding.UTF8, "application/json"),

                Method = _method!,
                RequestUri = new Uri(GetFinalUrl()),
            };
        }

        private string GetRequestBody()
        {
            return JsonSerializer.Serialize(_req, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        private string GetFinalUrl()
        {
            var url = _url;
            if (!Query.Any()) return url!;
            var queryString = GetQueryString();
            url += "?" + queryString;

            return url!;
        }

        public void AddHeader(string key, string value)
        {
            _headers[key] = value;
        }

        public void SetQueryParams(Dictionary<string, string> parameters)
        {
            Query = parameters;
        }

        private string? GetQueryString()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var queryS in Query)
            {
                query[queryS.Key] = queryS.Value;
            }

            return query.ToString();
        }
    }
}