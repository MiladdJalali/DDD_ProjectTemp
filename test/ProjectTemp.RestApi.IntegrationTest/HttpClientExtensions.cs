using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectTemp.RestApi.IntegrationTest
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string url, object body)
        {
            return client.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
        }

        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string url, object? parameters = null)
        {
            if (parameters is null)
                return client.GetAsync(url);

            var queryString = string.Join("&",
                parameters.GetType().GetProperties().Select(i => $"{i.Name}={i.GetValue(parameters)}"));

            return client.GetAsync($"{url}?{queryString}");
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient client, string url, object body)
        {
            return client.PutAsync(url,
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
        }

        public static async Task<T?> ReadAsync<T>(this HttpResponseMessage responseMessage)
        {
            var contentString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(contentString);
        }
    }
}