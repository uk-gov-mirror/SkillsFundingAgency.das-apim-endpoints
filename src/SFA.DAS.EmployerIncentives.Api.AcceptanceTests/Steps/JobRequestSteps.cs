﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Models.PassThrough;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "JobRequests")]
    public class JobRequestSteps
    {
        private readonly TestContext _context;
        private JobRequest _request;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public JobRequestSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to start a RefreshLegalEntities EmployerIncentives Job")]
        public void GivenTheCallerWantsToStartArefreshLegalEntitiesJob()
        {
            _request = new JobRequest
            {
                Type = JobType.RefreshLegalEntities,
                 Data = new Dictionary<string, object>
                {
                    { "PageNumber", _fixture.Create<int>() },
                    { "PageSize", _fixture.Create<int>() }
                }
            };
        }

        [Given(@"the Employer Incentives Api receives the Job request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheJobRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.NoContent;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/jobs")
                        .WithBody(new JsonMatcher(JsonSerializer.Serialize(_request), true))
                        .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }

        [When(@"the Outer Api receives the Job request")]
        public async Task WhenTheOuterApiReceivesTheJobRequest()
        {
           _response = await  _context.OuterApiClient.PutAsJsonAsync($"jobs", _request);
        }

        [Then(@"the response from the Employer Incentives Inner Api is returned")]
        public async Task ThenReturnTheResultFromEmployerIncentivesApiToTheCaller()
        {
            _response.StatusCode.Should().Be(_innerResponseStatusCode);
            var body = await _response.Content.ReadAsStringAsync();
            body.Should().BeNullOrWhiteSpace();
        }
    }
}
