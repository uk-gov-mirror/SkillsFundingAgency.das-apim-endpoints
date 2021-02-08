using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class CommitmentsV2Service
    {
        private readonly IInternalApiClient<CommitmentsV2Configuration> _client;
        private readonly ILogger<CommitmentsV2Service> _logger;

        public CommitmentsV2Service(
            IInternalApiClient<CommitmentsV2Configuration> client,
            ILogger<CommitmentsV2Service> logger) =>
            (_client, _logger) = (client, logger);

        public async Task<ApprenticeshipResponse> GetApprenticeshipDetails(long accountId, long apprenticeshipId)
        {
            var apprenticeship = await _client.Get<ApprenticeshipResponse>(
                new GetApprenticeshipDetailsRequest(apprenticeshipId));

            if (apprenticeship == null)
            {
                throw new HttpRequestContentException(
                    $"Apprenticeship Id {apprenticeshipId} not found",
                    System.Net.HttpStatusCode.BadRequest,
                    "");
            }

            _logger.LogInformation($"Entire response: {JsonConvert.SerializeObject(apprenticeship, Formatting.Indented)}");

            if (apprenticeship.EmployerAccountId != accountId)
            {
                throw new HttpRequestContentException(
                    $"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}",
                    System.Net.HttpStatusCode.BadRequest,
                    "");
            }

            return apprenticeship;
        }
    }
}