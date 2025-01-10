FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8004

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY FileService/FileService.Api/FileService.Api.csproj FileService/FileService.Api/
COPY FileService/FileService.Application/FileService.Application.csproj FileService/FileService.Application/
COPY FileService/FileService.Domain/FileService.Domain.csproj FileService/FileService.Domain/
COPY FileService/FileService.Infrastructure/FileService.Infrastructure.csproj FileService/FileService.Infrastructure/
COPY FileService/FileService.Persistence/FileService.Persistence.csproj FileService/FileService.Persistence/
COPY Shared/Shared.Application/Shared.Application.csproj Shared/Shared.Application/
COPY Shared/Shared.DTO/Shared.DTO.csproj Shared/Shared.DTO/

RUN dotnet restore FileService/FileService.Api/FileService.Api.csproj

COPY . .

WORKDIR /src/FileService/FileService.Api

RUN dotnet build FileService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish FileService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FileService.Api.dll"]