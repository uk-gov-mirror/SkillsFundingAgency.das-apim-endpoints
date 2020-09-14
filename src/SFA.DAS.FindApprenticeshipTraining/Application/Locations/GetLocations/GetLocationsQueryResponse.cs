﻿using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQueryResponse
    {
        public IEnumerable<GetLocationsListItem> Locations { get ; set ; }
    }
}