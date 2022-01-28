using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Model.Dtos.Responses;

namespace CustomerService.Clients.AuthClients
{
    public interface IAuthClient
    {
        Task<Response<TokenHandlerResponse>> TokenValidate(string token);
    }
}