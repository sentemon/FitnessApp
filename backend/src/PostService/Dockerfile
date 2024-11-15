FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY backend/src/PostService/PostService.Api/PostService.Api.csproj backend/src/PostService/PostService.Api/
COPY backend/src/PostService/PostService.Application/PostService.Application.csproj backend/src/PostService/PostService.Application/
COPY backend/src/PostService/PostService.Domain/PostService.Domain.csproj backend/src/PostService/PostService.Domain/
COPY backend/src/PostService/PostService.Infrastructure/PostService.Infrastructure.csproj backend/src/PostService/PostService.Infrastructure/
COPY backend/src/PostService/PostService.Persistence/PostService.Persistence.csproj backend/src/PostService/PostService.Persistence/
COPY backend/src/Shared/Shared.Application/Shared.Application.csproj backend/src/Shared/Shared.Application/

RUN dotnet restore backend/src/PostService/PostService.Api/

COPY . .

WORKDIR backend/src/PostService/PostService.Api
RUN dotnet build PostService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish PostService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PostService.Api.dll"]