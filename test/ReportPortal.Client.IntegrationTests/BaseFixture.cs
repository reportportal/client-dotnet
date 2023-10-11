using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.IntegrationTests
{
    public class BaseFixture
    {
        public Service Service { get; }

        public string Username { get; } = "default";

        public string ProjectName { get; } = "default_personal";

        public BaseFixture()
        {
            var url = "https://demo.reportportal.io";
            //var url = "http://localhost:8080";

            var httpCllientHandler = new HttpClientHandler();
            httpCllientHandler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;

            using (var httpClient = new HttpClient(httpCllientHandler))
            {
                httpClient.BaseAddress = new Uri(url);

                using (var uiTokenRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/uat/sso/oauth/token"))
                {
                    uiTokenRequestMessage.Content = new StringContent("grant_type=password&username=default&password=1q2w3e", Encoding.UTF8, "application/x-www-form-urlencoded");
                    uiTokenRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("ui:uiman")));

                    using (var uiTokenResponseMessage = httpClient.SendAsync(uiTokenRequestMessage).GetAwaiter().GetResult())
                    {
                        uiTokenResponseMessage.EnsureSuccessStatusCode();

                        var uiTokenJson = uiTokenResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        var uiToken = JsonSerializer.Deserialize<TokenModel>(uiTokenJson);

                        Service = new Service(new Uri($"{url}/api/v1"), ProjectName, uiToken.AccessToken);
                    }
                }
            }
        }

        class TokenModel
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
        }
    }
}
