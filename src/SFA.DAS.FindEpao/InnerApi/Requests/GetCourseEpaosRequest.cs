﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetCourseEpaosRequest : IGetApiRequest
    {
        public string CourseId { get; set; }
        public string GetUrl => $"api/v1/standards/{CourseId}/organisations";
    }
}