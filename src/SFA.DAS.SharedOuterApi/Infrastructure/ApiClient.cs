﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class ApiClient<T> : IApiClient<T> where T : IInnerApiConfiguration
    {
        private HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly T _configuration;

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
            _hostingEnvironment = hostingEnvironment;
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _configuration = apiConfiguration;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await _httpClient.PostAsync(request.PostUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.DeleteAsync(request.DeleteUrl)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await _httpClient.PatchAsync(request.PatchUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Put(IPutApiRequest request)
        
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await _httpClient.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Put<TData>(IPutApiRequest<TData> request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await _httpClient.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);
            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.GetAsync(request.GetAllUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<TResponse>>(json);
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request, string namedClient = default)
        {
            if(!string.IsNullOrEmpty(namedClient))
            {
                _httpClient = _httpClientFactory.CreateClient(namedClient);
            }

            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);
            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            return response.StatusCode;
        }

        private async Task AddAuthenticationHeader()
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.Identifier);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private void AddVersionHeader(string requestVersion)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Version");
            _httpClient.DefaultRequestHeaders.Add("X-Version", requestVersion);
        }

        public void UseNamedClient(string namedClient = null)
        {
            if (!string.IsNullOrEmpty(namedClient))
            {
                _httpClient = _httpClientFactory.CreateClient(namedClient);
            }
        }
    }
}
