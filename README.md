<img align="left" src="pozitronlogo.png" width="120" height="120">

&nbsp; [![NuGet](https://img.shields.io/nuget/v/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)[![NuGet](https://img.shields.io/nuget/dt/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)

&nbsp; [![Build Status](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_apis/build/status/fiseni.PozitronDev.QuerySpecification?branchName=master)](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_build/latest?definitionId=4&branchName=master)

&nbsp; [![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/pozitrondev/PozitronDev.QuerySpecification/4.svg)](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_build/latest?definitionId=4&branchName=master)

# PozitronDev QuerySpecification

Nuget package for building query specifications in your domain model. They are evaluated and utilized to create actual queries for your ORM.

This is base/abstract package intended to be utilized for various ORM implementation packages. Please check [PozitronDev.QuerySpecification](https://github.com/fiseni/QuerySpecificationEF) for the EF implementation, which it contains EF evaluators and generic repository ready to be consumed in your projects.

Note: The package uses the base premises of the following github project [Ardalis.Specification](https://github.com/fiseni/QuerySpecificationEF). Due to the many breaking changes introduced here, and different infrastructure and usage, I decided to maintain it as a separate package for now. Feel free to check both packages and use them as you desire.
