FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FitnessApp.sln", "./"]
COPY ["src/PostService/PostService.Api/PostService.Api.csproj", "src/PostService/PostService.Api/"]
COPY ["src/PostService/PostService.Application/PostService.Application.csproj", "src/PostService/PostService.Application/"]
COPY ["src/PostService/PostService.Domain/PostService.Domain.csproj", "src/PostService/PostService.Domain/"]
COPY ["src/PostService/PostService.Infrastructure/PostService.Infrastructure.csproj", "src/PostService/PostService.Infrastructure/"]
COPY ["src/PostService/PostService.Persistence/PostService.Persistence.csproj", "src/PostService/PostService.Persistence/"]
COPY ["src/Shared/Shared.Application/Shared.Application.csproj", "src/Shared/Shared.Application/"]

RUN dotnet restore "src/PostService/PostService.Api/PostService.Api.csproj"

COPY . .

WORKDIR "/src/src/PostService/PostService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PostService.Api.dll"]