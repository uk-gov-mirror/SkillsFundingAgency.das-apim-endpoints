using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InnerApi.Responses
{
    public class GetSectorsListResponse
    {
        public IEnumerable<GetSectorsListItem> Sectors { get; set; }
    }
}