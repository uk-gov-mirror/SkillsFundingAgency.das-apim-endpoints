﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Infrastructure;
using SFA.DAS.FindApprenticeshipTraining.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Infrastructure.HealthCheck
{
    public class WhenCallingPingOnTheCourseDeliveryApiClient
    {
        [Test, MoqAutoData]
        public async Task Then_The_Ping_Endpoint_Is_Called_For_CourseDeliveryApi(
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseDeliveryApiHealthCheck healthCheck)
        {
            //Act
            await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            client.Verify(x => x.GetResponseCode(It.IsAny<GetPingRequest>(), null), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Successful_200_Is_Returned(
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseDeliveryApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>(), null))
                .ReturnsAsync(HttpStatusCode.OK);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreEqual(HealthStatus.Healthy, actual.Status);
        }

        [Test, MoqAutoData]
        public async Task And_CoursesApi_Ping_Not_Found_Then_Unhealthy(
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseDeliveryApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(new GetPingRequest(), null))
                .ReturnsAsync(HttpStatusCode.NotFound);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreEqual(HealthStatus.Unhealthy, actual.Status);
        }
    }
}