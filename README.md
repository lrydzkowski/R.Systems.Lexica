# R.Systems.Lexica

R.Systems backend module called Lexica. It's written in ASP.NET Core 7 (C# language). Lexica is the English vocabulary learning software.

You can find here the following projects:

- R.Systems.Lexica.Api.Web - ASP.NET Core Web API, .NET 7, C# language
- R.Systems.Lexica.Infrastructure.AzureFiles - Class library, .NET 7, C# language. Library containing code responsible for communication with file system.
- R.Systems.Lexica.Core - Class library, .NET 7, C# language. Core functionalities of R.Systems.Lexica.
- R.Systems.Lexica.Tests.Api.Web.Integration - xUnit tests, .NET 7, C# language. Integration tests for endpoints available in R.Systems.Lexica.Api.Web.

The architecture of this solution is based on "Clean Architecture":

[https://github.com/ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)

[https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

## Docker

### Build image

```powershell
docker build -t r-systems-lexica -f .\R.Systems.Lexica.WebApi\Dockerfile .
```

### Run container

```powershell
docker run -it --rm -p 8080:80 `
    r-systems-lexica /bin/bash
```
