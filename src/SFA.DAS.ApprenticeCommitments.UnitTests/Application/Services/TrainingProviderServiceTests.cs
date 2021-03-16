﻿using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.Application.Services
{

    [TestFixture]
    public class TrainingProviderServiceTests
    {
        [Test, AutoData]
        public async Task WhenNoTrainingProviderIsFound(
            long trainingProviderId,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            var sut = new TrainingProviderService(client.Object);
            Func<Task> func = async () => await sut.GetTrainingProviderDetails(trainingProviderId);

            func.Should().Throw<HttpRequestContentException>();
        }

        [Test, AutoData]
        public async Task WhenMultipleTrainingProvidersAreFound(
            long trainingProviderId,
            TrainingProviderResponse[] results,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            client.Setup(x => 
                x.Get<SearchResponse>(It.IsAny<GetTrainingProviderDetailsRequest>()))
                .ReturnsAsync(new SearchResponse { SearchResults = results });

            var sut = new TrainingProviderService(client.Object);
            
            Func<Task> func = async () => await sut.GetTrainingProviderDetails(trainingProviderId);

            func.Should().Throw<HttpRequestContentException>();
        }

        [Test, AutoData]
        public async Task WhenASingleTrainingProvidersIsFound(
            long trainingProviderId,
            TrainingProviderResponse result,
            [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
        {
            client.Setup(x =>
                    x.Get<SearchResponse>(It.IsAny<GetTrainingProviderDetailsRequest>()))
                .ReturnsAsync(new SearchResponse {SearchResults = new[] {result}});

            var sut = new TrainingProviderService(client.Object);

            var response = await sut.GetTrainingProviderDetails(trainingProviderId);

            response.Should().Be(result);
        }
    }
}
