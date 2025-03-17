FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8006

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ChatService/ChatService.Api/ChatService.Api.csproj ChatService/ChatService.Api/
COPY ChatService/ChatService.Application/ChatService.Application.csproj ChatService/ChatService.Application/
COPY ChatService/ChatService.Domain/ChatService.Domain.csproj ChatService/ChatService.Domain/
COPY ChatService/ChatService.Infrastructure/ChatService.Infrastructure.csproj ChatService/ChatService.Infrastructure/
COPY ChatService/ChatService.Persistence/ChatService.Persistence.csproj ChatService/ChatService.Persistence/

COPY Shared/Shared.Application/Shared.Application.csproj Shared/Shared.Application/
COPY Shared/Shared.Authentication/Shared.Authentication.csproj Shared/Shared.Authentication/
COPY Shared/Shared.DTO/Shared.DTO.csproj Shared/Shared.DTO/

RUN dotnet restore ChatService/ChatService.Api/ChatService.Api.csproj

COPY . .

WORKDIR /src/ChatService/ChatService.Api

RUN dotnet build ChatService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ChatService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ChatService.Api.dll"]