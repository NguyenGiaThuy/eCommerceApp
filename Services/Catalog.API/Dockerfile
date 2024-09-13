FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 5000 5050

ENV ASPNETCORE_URLS=https://+:5050;http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Catalog.API/Catalog.API.csproj", "Services/Catalog.API/"]
RUN dotnet restore "Services/Catalog.API/Catalog.API.csproj"
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj"
COPY ["BuildingBlocks/BuildingBlocks/", "BuildingBlocks/BuildingBlocks/"]
COPY . .
WORKDIR "/src/Services/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Catalog.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
