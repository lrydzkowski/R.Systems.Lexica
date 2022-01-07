FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./R.Systems.Lexica.Core/.", "R.Systems.Lexica.Core/"]
COPY ["./R.Systems.Lexica.Infrastructure/.", "R.Systems.Lexica.Infrastructure/"]
COPY ["./R.Systems.Lexica.WebApi/.", "R.Systems.Lexica.WebApi/"]
WORKDIR "/src/R.Systems.Lexica.WebApi"
RUN dotnet restore "R.Systems.Lexica.WebApi.csproj"
RUN dotnet build "R.Systems.Lexica.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "R.Systems.Lexica.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "R.Systems.Lexica.WebApi.dll"]