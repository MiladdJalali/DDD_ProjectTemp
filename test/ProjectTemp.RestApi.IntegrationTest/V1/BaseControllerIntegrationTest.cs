using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using ProjectTemp.RestApi.V1.Models;

namespace ProjectTemp.RestApi.IntegrationTest.V1
{
    public abstract class BaseControllerIntegrationTest
    {
        protected BaseControllerIntegrationTest(TestFixture testFixture)
        {
            HttpClient = testFixture.HttpClient;
        }

        protected HttpClient HttpClient { get; }

        protected abstract string BaseUrl { get; }

        protected async Task<T> Create<T>(object request, HttpStatusCode responseStatusCode = HttpStatusCode.Created)
            where T : class
        {
            var response = await HttpClient.PostAsJsonAsync(BaseUrl, request);

            response.StatusCode.Should().Be(responseStatusCode);

            return responseStatusCode != HttpStatusCode.Created
                ? null
                : (await response.ReadAsync<ResponseModel<T>>()).Values;
        }

        protected async Task<T> Clone<T>(
            string identifier,
            object request,
            HttpStatusCode responseStatusCode = HttpStatusCode.Created)
            where T : class
        {
            var response = await HttpClient
                .PostAsJsonAsync($"{BaseUrl}/{HttpUtility.UrlEncode(identifier)}/Clone", request);

            response.StatusCode.Should().Be(responseStatusCode);

            return responseStatusCode != HttpStatusCode.Created
                ? null
                : (await response.ReadAsync<ResponseModel<T>>()).Values;
        }

        protected async Task<T> GetByIdentifier<T>(string identifier,
            HttpStatusCode responseStatusCode = HttpStatusCode.OK)
            where T : class
        {
            var response = await HttpClient.GetAsync($"{BaseUrl}/{HttpUtility.UrlEncode(identifier)}");

            response.StatusCode.Should().Be(responseStatusCode);

            return responseStatusCode != HttpStatusCode.OK
                ? null
                : (await response.ReadAsync<ResponseModel<T>>()).Values;
        }

        protected async Task<ResponseCollectionModel<T>> GetAll<T>(object parameters,
            HttpStatusCode responseStatusCode = HttpStatusCode.OK)
            where T : class
        {
            var response = await HttpClient.GetAsync(BaseUrl, parameters);

            response.StatusCode.Should().Be(responseStatusCode);

            return responseStatusCode != HttpStatusCode.OK
                ? null
                : await response.ReadAsync<ResponseCollectionModel<T>>();
        }

        protected async Task Update(
            string identifier,
            object request,
            HttpStatusCode responseStatusCode = HttpStatusCode.NoContent)
        {
            var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{HttpUtility.UrlEncode(identifier)}", request);

            response.StatusCode.Should().Be(responseStatusCode);
        }

        protected async Task Update(
            object request,
            HttpStatusCode responseStatusCode = HttpStatusCode.NoContent)
        {
            var response = await HttpClient.PutAsJsonAsync(BaseUrl, request);

            response.StatusCode.Should().Be(responseStatusCode);
        }

        protected async Task Delete(string identifier, HttpStatusCode responseStatusCode = HttpStatusCode.OK)
        {
            var response = await HttpClient.DeleteAsync($"{BaseUrl}/{HttpUtility.UrlEncode(identifier)}");

            response.StatusCode.Should().Be(responseStatusCode);
        }
    }
}