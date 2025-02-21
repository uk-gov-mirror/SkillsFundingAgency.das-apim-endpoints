# das-apim-endpoints

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-apim-endpoints?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2208&branchName=master)

## Requirements

* DotNet Core 3.1 and any supported IDE for DEV running.
* Azure Storage Account

## About

das-apim-endpoints covers the outer apis that reside within the APIM gateway. All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.  

## Local running

### Find Apprenticeship Training

The Find Apprenticeship Training outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-coursedelivery-api](https://github.com/SkillsFundingAgency/das-coursedelivery-api)
* [das-location-api](https://github.com/SkillsFundingAgency/das-location-api)
* [das-employerdemand-api](https://github.com/SkillsFundingAgency/das-employerdemand-api)


You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeshipTraining.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "CourseDeliveryApiConfiguration" : {
        "url":"https://localhost:5006/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "LocationApiConfiguration" : {
        "url":"https://localhost:5008/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "EmployerDemandApiConfiguration":{
        "url":"https://localhost:5501/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.FindApprenticeshipTraining.Api```

Starting the API will then show the swagger definition with the available operations. Alternatively you can connect [das-findapprenticeshiptraining](https://github.com/SkillsFundingAgency/das-findapprenticeshiptraining) which is the consuming service of this outer API.

### Find an Endpoint Assessment Organisation

The Find Endpoint Assessment Organisation outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-assessor-service](https://github.com/SkillsFundingAgency/das-assessor-service/)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindEpao.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "AssessorsApiConfiguration" : {
        "url":"http://localhost:59022/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.FindEpao.Api```

Starting the API will then show the swagger definition with the available operations. Alternatively you can connect [das-find-epao-web](https://github.com/SkillsFundingAgency/das-find-epao-web/) which is the consuming service of this outer API.

### Approvals

The Approvals outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-coursedelivery-api](https://github.com/SkillsFundingAgency/das-coursedelivery-api)
* [das-assessor-service](https://github.com/SkillsFundingAgency/das-assessor-service/)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Approvals.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "CourseDeliveryApiConfiguration" : {
        "url":"https://localhost:5006/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "AssessorsApiConfiguration" : {
        "url":"http://localhost:59022/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.Approvals.Api```

Starting the API will then show the swagger definition with the available operations. This outer API is used to provide data for the Commitments v2 webjobs that store assessment organisations, provider and course information.

### Campaign

The Campaign outer api relies on the following inner api:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Campaign.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.Campaign.Api```

Starting the API will then show the swagger definition with the available operations. The outer API is used for displaying the list of routes for Standards. Once a route is selected then all the standard Ids in that route are returned.

### EPAO Register

The Epao register outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-assessor-service](https://github.com/SkillsFundingAgency/das-assessor-service/)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.EpaoRegister.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "AssessorsApiConfiguration" : {
        "url":"http://localhost:59022/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.EpaoRegister.Api```

Starting the API will then show the swagger definition with the available operations. This outer API is to be used as a resource for external subscribers to ESFA services, to get information about Endpoint Assessment Organisations and the courses that they can assess. 

### Forecasting

The forecasting outer api relies on the following inner api:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Forecasting.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    }
}
```
* Start the api project ```SFA.DAS.Forecasting.Api```

Starting the API will then show the swagger definition with the available operations. The outer API is used to populate standards and frameworks in the forecasting database.

### Manage Apprenticeships

The Manage Apprenticeships outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-coursedelivery-api](https://github.com/SkillsFundingAgency/das-coursedelivery-api)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.ManageApprenticeships.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "CourseDeliveryApiConfiguration" : {
        "url":"https://localhost:5006/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
}
```
* Start the api project ```SFA.DAS.ManageApprenticeships.Api```

Starting the API will then show the swagger definition with the available operations. The outer API is used during payment processing for transaction information, adding metadata to the payments lines for Course and Provider information.

### Recruit

The Recruit outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-coursedelivery-api](https://github.com/SkillsFundingAgency/das-coursedelivery-api)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Recruit.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "CourseDeliveryApiConfiguration" : {
        "url":"https://localhost:5006/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
}
```
* Start the api project ```SFA.DAS.Recruit.Api```

Starting the API will then show the swagger definition with the available operations. The outer API is used as part of the daily data refresh, to store provider, standard and frameworks information.

### Reservations

The Reservations outer api relies on the following inner apis:

* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)
* [das-coursedelivery-api](https://github.com/SkillsFundingAgency/das-coursedelivery-api)

You are able to run the API by doing the following:


* In your Azure Storage Account, create a table called Configuration and add the following. Note that the identifier is not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.Reservations.OuterApi_1.0
Data: {
    "CoursesApiConfiguration": {
        "url":"https://localhost:5001/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
    "CourseDeliveryApiConfiguration" : {
        "url":"https://localhost:5006/",
        "identifier":"https://**********.onmicrosoft.com/*******"
    },
}
```
* Start the api project ```SFA.DAS.Reservations.Api```

Starting the API will then show the swagger definition with the available operations. The outer API is used as part of the daily data refresh, to store provider, standard and frameworks information.


### Employer Incentives

When Running Employer Incentives Api Locally

You will need override the setting in the config ```SFA.DAS.EmployerIncentives.OuterApi_1.0``` with the following in appsettings.development.json

```
  {
  "Environment": "DEV",
  "EmployerIncentivesInnerApi": {
    "identifier": "",
    "url": "https://localhost:5001/",
  },
  "CommitmentsV2InnerApi": {
    "identifier": "",
    "url": "http://localhost:6011/"
  }
}
```

To invoke the Fake CommitmentsV2Api start the console application ```SFA.DAS.EmployerIncentives.FakeApis``` 

