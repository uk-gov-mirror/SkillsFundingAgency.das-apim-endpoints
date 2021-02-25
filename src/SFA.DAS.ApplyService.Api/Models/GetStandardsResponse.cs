using System.Collections.Generic;
using SFA.DAS.ApplyService.InnerApi.Responses;

namespace SFA.DAS.ApplyService.Api.Models
{
    public class GetStandardsResponse
    {
        public IEnumerable<GetStandardsResponseItem> Standards { get; set; }
    }

    public class GetStandardsResponseItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static implicit operator GetStandardsResponseItem(GetStandardsListItem source)
        {
            return new GetStandardsResponseItem
            {
                Id = source.Id,
                Title = source.Title
            };
        }
    }
}