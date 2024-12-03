FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ARG ASPNETCORE_ENVIRONMENT
ARG DB_CONNECTION_STRING

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__DockerConnection=${DB_CONNECTION_STRING}

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY PostService/PostService.Api/PostService.Api.csproj PostService/PostService.Api/
COPY PostService/PostService.Application/PostService.Application.csproj PostService/PostService.Application/
COPY PostService/PostService.Domain/PostService.Domain.csproj PostService/PostService.Domain/
COPY PostService/PostService.Infrastructure/PostService.Infrastructure.csproj PostService/PostService.Infrastructure/
COPY PostService/PostService.Persistence/PostService.Persistence.csproj PostService/PostService.Persistence/
COPY Shared/Shared.Application/Shared.Application.csproj Shared/Shared.Application/

RUN dotnet restore PostService/PostService.Api/PostService.Api.csproj

COPY . .

WORKDIR /src/PostService/PostService.Api

RUN dotnet build PostService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish PostService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PostService.Api.dll"]