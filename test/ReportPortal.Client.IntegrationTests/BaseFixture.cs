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

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/uat/sso/oauth/token");
                requestMessage.Content = new StringContent("grant_type=password&username=default&password=1q2w3e", Encoding.UTF8, "application/x-www-form-urlencoded");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("ui:uiman")));

                var response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var token = JsonSerializer.Deserialize<UiToken>(json);


                var r2 = new HttpRequestMessage(HttpMethod.Get, "uat/sso/me/apitoken");
                r2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                var rr2 = httpClient.SendAsync(r2).GetAwaiter().GetResult();
                rr2.EnsureSuccessStatusCode();

                var json2 = rr2.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var token2 = JsonSerializer.Deserialize<UiToken>(json2);

                Service = new Service(new Uri($"{url}/api/v1"), ProjectName, token2.AccessToken);
            }


        }

        class UiToken
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
        }
    }
}
