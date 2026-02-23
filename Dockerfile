FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ClickHouseDemo.sln .
COPY ClickHouse.Core/ClickHouse.Core.csproj ClickHouse.Core/
COPY ClickHouse.Data/ClickHouse.Data.csproj ClickHouse.Data/
COPY ClickHouse.API/ClickHouse.API.csproj ClickHouse.API/
RUN dotnet restore

COPY . .
RUN dotnet publish ClickHouse.API/ClickHouse.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ClickHouse.API.dll"]
