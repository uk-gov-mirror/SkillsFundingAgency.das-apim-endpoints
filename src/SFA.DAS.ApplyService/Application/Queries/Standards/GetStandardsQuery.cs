using MediatR;

namespace SFA.DAS.ApplyService.Application.Queries.Standards
{
    public class GetStandardsQuery : IRequest<GetStandardsQueryResult>
    {
        public string Sector { get; set; }
    }
}