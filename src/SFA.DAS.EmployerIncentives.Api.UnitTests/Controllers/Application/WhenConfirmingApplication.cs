﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    public class WhenConfirmingApplication
    {
        [Test, MoqAutoData]
        public async Task Then_ConfirmApplicationCommand_is_handled(
            long accountId,
            ConfirmApplicationRequest request,
            CreateAccountLegalEntityCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ConfirmApplicationCommand>(c =>
                        c.AccountId.Equals(accountId)
                        && c.AccountId.Equals(request.AccountId)
                        && c.ApplicationId.Equals(request.ApplicationId)
                        && c.DateSubmitted.Equals(request.DateSubmitted)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.ConfirmApplication(request) as StatusCodeResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}