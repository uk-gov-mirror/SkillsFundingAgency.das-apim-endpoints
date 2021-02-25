using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApplyService.InnerApi.Requests
{
    public class GetSectorsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/sectors";
    }
}
