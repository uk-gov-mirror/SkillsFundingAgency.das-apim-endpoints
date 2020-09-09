using System.Collections.Generic;
using System.Linq;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProviderByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_Provider_For_That_Course_From_Course_Delivery_Api_Client(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiResponse, apiCourseResponse, apiAchievementRateResponse, allCoursesApiResponse, mockCoursesApiClient, mockApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ProviderStandard.Should().BeEquivalentTo(apiResponse);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_The_Overall_Achievement_Rate_Data_From_The_Course_SubjectSectorArea(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiResponse, apiCourseResponse, apiAchievementRateResponse, allCoursesApiResponse, mockCoursesApiClient, mockApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.OverallAchievementRates.Should().BeEquivalentTo(apiAchievementRateResponse.OverallAchievementRates);
        }

        [Test, MoqAutoData]
        public async Task Then_Does_Not_Return_Additional_Courses_If_No_Additional_Courses(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProviderStandardItem>(It.Is<GetProviderByCourseAndUkPrnRequest>(c =>
                    c.GetUrl.Contains(query.CourseId.ToString())
                    && c.GetUrl.Contains(query.ProviderId.ToString()
                    )), true))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetProviderAdditionalStandardsItem>(It.Is<GetProviderAdditionalStandardsRequest>(c =>
                    c.GetUrl.Contains(query.ProviderId.ToString()
                    )), true))
                .ReturnsAsync(new GetProviderAdditionalStandardsItem
                {
                    StandardIds = new List<int>()
                });
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString())), true))
                .ReturnsAsync(apiCourseResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsListRequest>(), true))
                .ReturnsAsync(allCoursesApiResponse);
            mockApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(c =>
                    c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                ), true))
                .ReturnsAsync(apiAchievementRateResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Should().BeEquivalentTo(new List<GetAdditionalCourseListItem>());
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Additional_Courses_for_Provider_That_Are_Available(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProviderStandardItem>(It.Is<GetProviderByCourseAndUkPrnRequest>(c =>
                    c.GetUrl.Contains(query.CourseId.ToString())
                    && c.GetUrl.Contains(query.ProviderId.ToString()
                    )), true))
                .ReturnsAsync(apiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsListRequest>(), true))
                .ReturnsAsync(allCoursesApiResponse);

            var additionalCourses = allCoursesApiResponse.Standards.Select(c => c.Id).ToList();

            additionalCourses.Add(-10);

            mockApiClient
                .Setup(client => client.Get<GetProviderAdditionalStandardsItem>(It.Is<GetProviderAdditionalStandardsRequest>(
                    c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        )), true))
                .ReturnsAsync(new GetProviderAdditionalStandardsItem
                {
                    StandardIds = additionalCourses
                });

            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString())), true))
                .ReturnsAsync(apiCourseResponse);

            mockApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(
                    c =>
                        c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                ), true))
                .ReturnsAsync(apiAchievementRateResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Should().BeInAscendingOrder(c => c.Title);

            result.AdditionalCourses.Should()
                .BeEquivalentTo(allCoursesApiResponse.Standards, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Additional_Courses_for_Provider_In_Alphabetical_Order(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiResponse, apiCourseResponse, apiAchievementRateResponse, allCoursesApiResponse, mockCoursesApiClient, mockApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Should().BeInAscendingOrder(c=>c.Title);
            result.AdditionalCourses.Should()
                .BeEquivalentTo(allCoursesApiResponse.Standards, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_Additional_Courses_for_Provider_Should_Not_Contain_Course_Passed_To_Handler(
            GetTrainingCourseProviderQuery query,
            GetProviderStandardItem apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetProviderAdditionalStandardsItem apiAdditionalStandardsResponse,
            GetStandardsListResponse allCoursesApiResponse,
            List<GetStandardsListItem> allStandards,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            apiCourseResponse.Id = query.CourseId;
            allStandards.Add(new GetStandardsListItem
            {
                Id=apiCourseResponse.Id,
                Title = apiCourseResponse.Title,
                Level = apiCourseResponse.Level
            });
            allCoursesApiResponse.Standards = allStandards;
            ArrangeClients(query, apiResponse, apiCourseResponse, apiAchievementRateResponse, allCoursesApiResponse, mockCoursesApiClient, mockApiClient);
            

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Count().Should().Be(allCoursesApiResponse.Standards.Count()-1);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_The_Overall_Achievement_Rate_Data_From_The_Course_SubjectSectorArea(
            GetTrainingCourseProviderQuery query,
            GetOverallAchievementRateResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetStandardsListResponse allCoursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString())), true))
                .ReturnsAsync(apiCourseResponse);
            mockApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(c=>
                    c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                ), true))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetProviderAdditionalStandardsItem>(
                    It.Is<GetProviderAdditionalStandardsRequest>(c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        )), true))
                .ReturnsAsync(new GetProviderAdditionalStandardsItem
                {
                    StandardIds = allCoursesApiResponse.Standards.Select(c => c.Id).ToList()
                });
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsListRequest>(), true))
                .ReturnsAsync(allCoursesApiResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.OverallAchievementRates.Should().BeEquivalentTo(apiResponse.OverallAchievementRates);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_Location_In_The_Request_The_Location_Data_Is_Added_To_The_Get_Course_Provider_Request(
            string locationName,
            string authorityName,
            GetTrainingCourseProviderQuery query,
            GetOverallAchievementRateResponse apiResponse,
            GetProviderStandardItem apiProviderStandardResponse,
            GetStandardsListItem apiCourseResponse,
            GetStandardsListResponse allCoursesApiResponse,
            GetLocationsListItem apiLocationResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.Location = $"{locationName}, {authorityName} ";
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByLocationAndAuthorityName>(c => c.GetUrl.Contains(locationName.Trim()) && c.GetUrl.Contains(authorityName.Trim()))))
                .ReturnsAsync(apiLocationResponse);
            
            mockApiClient
                .Setup(client => client.Get<GetProviderStandardItem>(It.Is<GetProviderByCourseAndUkPrnRequest>(c =>
                    c.GetUrl.Contains(query.CourseId.ToString())
                    && c.GetUrl.Contains(query.ProviderId.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    )))
                .ReturnsAsync(apiProviderStandardResponse);
            mockApiClient
                .Setup(client => client.Get<GetProviderAdditionalStandardsItem>(It.Is<GetProviderAdditionalStandardsRequest>(
                    c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        ))))
                .ReturnsAsync(new GetProviderAdditionalStandardsItem
                {
                    StandardIds = allCoursesApiResponse.Standards.Select(c => c.Id).ToList()
                });
            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsListRequest>()))
                .ReturnsAsync(allCoursesApiResponse);
            mockApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(
                    c =>
                        c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                )))
                .ReturnsAsync(apiAchievementRateResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);
            
            result.ProviderStandard.Should().BeEquivalentTo(apiProviderStandardResponse);
        }

        private static void ArrangeClients(GetTrainingCourseProviderQuery query, GetProviderStandardItem apiProviderStandardResponse,
            GetStandardsListItem apiCourseResponse, GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetStandardsListResponse allCoursesApiResponse, Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient, Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient)
        {
            mockApiClient
                .Setup(client => client.Get<GetProviderStandardItem>(It.Is<GetProviderByCourseAndUkPrnRequest>(c =>
                    c.GetUrl.Contains(query.CourseId.ToString())
                    && c.GetUrl.Contains(query.ProviderId.ToString()
                    )), true))
                .ReturnsAsync(apiProviderStandardResponse);
            
            mockApiClient
                .Setup(client => client.Get<GetProviderAdditionalStandardsItem>(It.Is<GetProviderAdditionalStandardsRequest>(
                    c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        )), true))
                .ReturnsAsync(new GetProviderAdditionalStandardsItem
                {
                    StandardIds = allCoursesApiResponse.Standards.Select(c => c.Id).ToList()
                });
            
            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString())), true))
                .ReturnsAsync(apiCourseResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsListRequest>(), true))
                .ReturnsAsync(allCoursesApiResponse);
            mockApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(
                    c =>
                        c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                ), true))
                .ReturnsAsync(apiAchievementRateResponse);
        }
    }
}