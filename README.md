# R.Systems.Lexica

R.Systems backend module called Lexica. It's written in ASP.NET Core 6 (C# language).
Lexica is the English vocabulary learning software.

You can find here the following projects:

- R.Systems.Lexica.WebApi - ASP.NET Core Web API, .NET 6, C# language
- R.Systems.Lexica.Infrastructure - Class library, .NET 6, C# language. Library containing code responsible for communication with file system.
- R.Systems.Lexica.Core - Class library, .NET 6, C# language. Core functionalities of R.Systems.Lexica.
- R.Systems.Lexica.FunctionalTests - xUnit tests, .NET 6, C# language. E2E tests for endpoints available in R.Systems.Lexica.WebApi.

This project is used by the web frontend written in Angular:
[R.Systems.WebFrontend](https://github.com/lrydzkowski/R.Systems.WebFrontend).

The architecture of this solution is based on "Clean Architecture":

[https://github.com/ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)

[https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)
