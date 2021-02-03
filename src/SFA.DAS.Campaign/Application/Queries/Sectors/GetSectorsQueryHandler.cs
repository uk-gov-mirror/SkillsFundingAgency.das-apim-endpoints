﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Sectors
{
    public class GetSectorsQueryHandler : IRequestHandler<GetSectorsQuery, GetSectorsQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetSectorsQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetSectorsQueryResult> Handle(GetSectorsQuery request, CancellationToken cancellationToken)
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetSectorsListResponse>(nameof(GetSectorsListResponse));
            if (response == null)
            {
                response = await _coursesApiClient.Get<GetSectorsListResponse>(new GetSectorsListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetSectorsListResponse), response, 23);
            }
            
            return new GetSectorsQueryResult
            {
                Sectors = response.Sectors
            };
        }
    }
}