﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplications;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    [TestFixture]
    public class WhenGettingApplicationsForAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Applications_From_Mediator(
            long accountId,
            GetApplicationsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationsQuery>(c =>
                        c.AccountId.Equals(accountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApplications(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as IEnumerable<ApprenticeApplication>;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.ApprenticeApplications);
        }
    }
}
