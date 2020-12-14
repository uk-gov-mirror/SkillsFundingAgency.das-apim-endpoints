﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.WithdrawApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.JobControllerTests
{
    [TestFixture]
    public class WhenWithdrawRequest
    {
        [Test, MoqAutoData]
        public async Task Then_AddJobCommand_is_handled(
            WithdrawRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] WithdrawlController controller)
        {
            var controllerResult = await controller.Withdraw(request) as AcceptedResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<WithdrawCommand>(c =>
                        c.WithdrawRequest.Equals(request)),
                    It.IsAny<CancellationToken>()));

            Assert.IsNotNull(controllerResult);
        }
    }
}
