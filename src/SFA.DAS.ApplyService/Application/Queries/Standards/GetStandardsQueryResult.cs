using System.Collections.Generic;
using SFA.DAS.ApplyService.InnerApi.Responses;

namespace SFA.DAS.ApplyService.Application.Queries.Standards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}