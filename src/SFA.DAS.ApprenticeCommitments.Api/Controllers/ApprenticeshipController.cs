using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeshipController(IMediator mediator, IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
        {
            _mediator = mediator;
            _client = client;
        }

        [HttpPost]
        [Route("/apprenticeships")]
        public async Task<IActionResult> AddApprenticeship(CreateApprenticeshipCommand request)
        {
            await _mediator.Send(request);
            return Accepted();
        }

        [HttpGet("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public async Task<IActionResult> GetApprenticeship(Guid apprenticeId, long apprenticeshipId)
        {
            var response = await _client.Get<ApprenticeshipResponse>(new GetApprenticeshipRequest(apprenticeId, apprenticeshipId));

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/trainingproviderconfirmation")]
        public async Task<IActionResult> TrainingProviderConfirmation(
            Guid apprenticeId, long apprenticeshipId,
            [FromBody] TrainingProviderConfirmationRequestData request)
        {
            await _client.Post(
                new TrainingProviderConfirmationRequest(
                    apprenticeId, apprenticeshipId, request.TrainingProviderCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/employerconfirmation")]
        public async Task<IActionResult> EmployerConfirmation(
            Guid apprenticeId, long apprenticeshipId,
            [FromBody] EmployerConfirmationRequestData request)
        {
            await _client.Post(
                new EmployerConfirmationRequest(
                    apprenticeId, apprenticeshipId, request.EmployerCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeid}/apprenticeships/{apprenticeshipid}/apprenticeshipdetailsconfirmation")]
        public async Task<IActionResult> ApprenticeshipConfirmation(
            Guid apprenticeId, long apprenticeshipId,
            [FromBody] EmployerConfirmationRequestData request)
        {
            await _client.Post(
                new ApprenticeshipDetailsConfirmationRequest(
                    apprenticeId, apprenticeshipId, request.EmployerCorrect));

            return Ok();
        }

        [HttpPost("/apprentices/{apprenticeId}/email")]
        public async Task<IActionResult> ChangeApprenticeEmailAddress(
            Guid apprenticeId,
            ApprenticeEmailAddressRequest request)
        {
            await _mediator.Send(
                new ChangeEmailAddressCommand(apprenticeId, request.Email));
            return Ok();
        }
    }
}