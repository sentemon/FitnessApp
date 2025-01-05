FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8001

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY AuthService/AuthService.Api/AuthService.Api.csproj AuthService/AuthService.Api/
COPY AuthService/AuthService.Application/AuthService.Application.csproj AuthService/AuthService.Application/
COPY AuthService/AuthService.Domain/AuthService.Domain.csproj AuthService/AuthService.Domain/
COPY AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService/AuthService.Infrastructure/
COPY AuthService/AuthService.Persistence/AuthService.Persistence.csproj AuthService/AuthService.Persistence/
COPY Shared/Shared.Application/Shared.Application.csproj Shared/Shared.Application/
COPY Shared/Shared.DTO/Shared.DTO.csproj Shared/Shared.DTO/

RUN dotnet restore AuthService/AuthService.Api/AuthService.Api.csproj

COPY . .

WORKDIR /src/AuthService/AuthService.Api

RUN dotnet build AuthService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish AuthService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AuthService.Api.dll"]