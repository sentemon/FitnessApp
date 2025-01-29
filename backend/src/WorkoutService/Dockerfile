FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8005

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY WorkoutService/WorkoutService.Api/WorkoutService.Api.csproj WorkoutService/WorkoutService.Api/
COPY WorkoutService/WorkoutService.Application/WorkoutService.Application.csproj WorkoutService/WorkoutService.Application/
COPY WorkoutService/WorkoutService.Domain/WorkoutService.Domain.csproj WorkoutService/WorkoutService.Domain/
COPY WorkoutService/WorkoutService.Infrastructure/WorkoutService.Infrastructure.csproj WorkoutService/WorkoutService.Infrastructure/
COPY WorkoutService/WorkoutService.Persistence/WorkoutService.Persistence.csproj WorkoutService/WorkoutService.Persistence/

COPY Shared/Shared.Application/Shared.Application.csproj Shared/Shared.Application/
COPY Shared/Shared.Authentication/Shared.Authentication.csproj Shared/Shared.Authentication/
COPY Shared/Shared.DTO/Shared.DTO.csproj Shared/Shared.DTO/

RUN dotnet restore WorkoutService/WorkoutService.Api/WorkoutService.Api.csproj

COPY . .

WORKDIR /src/WorkoutService/WorkoutService.Api

RUN dotnet build WorkoutService.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WorkoutService.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WorkoutService.Api.dll"]