using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderAdditionalStandardsItem
    {
        public IEnumerable<int> CourseIds { get; set; }
    }
}