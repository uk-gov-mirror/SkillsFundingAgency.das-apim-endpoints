using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Clients
{
    public class EmployerIncentivesApiClient : IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>
    {
        private readonly IApiClient<EmployerIncentivesConfiguration> _client;

        public EmployerIncentivesApiClient(IApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _client.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _client.GetAll<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request, string namedClient = null)
        {
            return _client.GetResponseCode(request, namedClient);
        }
        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return _client.Post<TResponse>(request);
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _client.Delete(request);
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            return _client.Patch(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return _client.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _client.Put(request);
        }
    }
}