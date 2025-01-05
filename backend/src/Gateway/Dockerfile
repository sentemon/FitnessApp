FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Gateway/Gateway/Gateway.csproj Gateway/Gateway/

RUN dotnet restore Gateway/Gateway/Gateway.csproj

COPY . .

WORKDIR /src/Gateway/Gateway

RUN dotnet build Gateway.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Gateway.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Gateway.dll"]