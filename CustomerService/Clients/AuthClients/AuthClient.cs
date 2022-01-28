using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Model.Dtos.Responses;
using Newtonsoft.Json;

namespace CustomerService.Clients.AuthClients
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;

        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<TokenHandlerResponse>> TokenValidate(string token)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(AuthClientSettings.ValidateTokenUrl+token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthApiResponse>(responseBody);
                return new SuccessResponse<TokenHandlerResponse>(result.Data);
            }
            throw new UnauthorizedAccessException("Error token");
        }
    }
}