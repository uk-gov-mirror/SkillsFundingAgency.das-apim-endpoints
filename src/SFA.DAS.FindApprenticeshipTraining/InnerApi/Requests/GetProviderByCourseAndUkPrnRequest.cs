using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderByCourseAndUkPrnRequest : IGetApiRequest
    {
        private readonly int _providerId;
        private readonly int _courseId;
        private readonly double? _latitude;
        private readonly double? _longitude;

        public GetProviderByCourseAndUkPrnRequest(int providerId, int courseId, double? latitude = null, double? longitude = null)
        {
            _providerId = providerId;
            _courseId = courseId;
            _latitude = latitude;
            _longitude = longitude;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/{_courseId}/providers/{_providerId}?lat={_latitude}&lon={_longitude}";
    }
}