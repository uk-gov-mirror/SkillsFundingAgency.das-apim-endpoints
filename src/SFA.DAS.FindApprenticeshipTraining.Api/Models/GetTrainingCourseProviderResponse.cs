using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderResponse
    {
        public GetTrainingCourseListItem TrainingCourse { get ; set ; }
        public GetProviderCourseItem TrainingCourseProvider { get; set; }
        public List<GetAdditionalCourseListItem> AdditionalCourses { get; set; }
    }
}