# R.Systems.Lexica

R.Systems backend module called Lexica. It's written in ASP.NET Core 7 (C# language). Lexica is the English vocabulary learning software.

You can find here the following projects:

- R.Systems.Lexica.Api.Web - Web API
- R.Systems.Lexica.Infrastructure.Azure - Code connected with using Azure.
- R.Systems.Lexica.Infrastructure.Db - Code connected with using a database (EF Core + PostgreSQL).
- R.Systems.Lexica.Infrastructure.EnglishDictionary - Code containing communication with the external English dictionary.
- R.Systems.Lexica.Infrastructure.Storage - Code connected with saving recordings.
- R.Systems.Lexica.Infrastructure.Wordnik - Code containing communication with Wordnik API.
- R.Systems.Lexica.Core - Core functionalities.
- R.Systems.Lexica.Tests.Api.Web.Integration - Integration tests for endpoints available in R.Systems.Lexica.Api.Web.

The architecture of this solution is based on "Clean Architecture":

[https://github.com/ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)

[https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

## Front-end

<https://github.com/lrydzkowski/R.Systems.ReactFront>
