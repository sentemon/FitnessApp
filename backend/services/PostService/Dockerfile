FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PostService.sln", "./"]
COPY ["src/PostService.Api/PostService.Api.csproj", "src/PostService.Api/"]
COPY ["src/PostService.Application/PostService.Application.csproj", "src/PostService.Application/"]
COPY ["src/PostService.Domain/PostService.Domain.csproj", "src/PostService.Domain/"]
COPY ["src/PostService.Infrastructure/PostService.Infrastructure.csproj", "src/PostService.Infrastructure/"]
COPY ["src/PostService.Persistence/PostService.Persistence.csproj", "src/PostService.Persistence/"]

RUN dotnet restore "src/PostService.Api/PostService.Api.csproj"

COPY . .

WORKDIR "/src/src/PostService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PostService.Api.dll"]