﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetAggregatedCourseDemandListRequest
    {
        [Test, AutoData]
        public void Then_Sets_Url_Correctly(
            int ukprn,
            int courseId,
            double lat,
            double lon,
            int radius)
        {
            var request = new GetAggregatedCourseDemandListRequest(ukprn, courseId, lat, lon, radius);
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}&lat={request.Lat}&lon={request.Lon}&radius={request.Radius}");
            request.CourseId.Should().Be(courseId);
            request.Lat.Should().Be(lat);
            request.Lon.Should().Be(lon);
            request.Radius.Should().Be(radius);
        }
    }
}