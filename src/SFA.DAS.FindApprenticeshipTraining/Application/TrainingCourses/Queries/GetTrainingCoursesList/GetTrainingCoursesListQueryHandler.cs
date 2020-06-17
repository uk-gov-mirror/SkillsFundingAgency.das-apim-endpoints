﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQueryHandler : IRequestHandler<GetTrainingCoursesListQuery, GetTrainingCoursesListResult>
    {
        private readonly IApiClient _apiClient;

        public GetTrainingCoursesListQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = _apiClient.GetAll<GetStandardsListItem>(new GetStandardsListRequest());
            //todo: sectors, levels here

            await Task.WhenAll(standardsTask);

            return new GetTrainingCoursesListResult
            {
                Courses = standardsTask.Result
            };
        }
    }
}
