FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install curl in the base image to use healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app
EXPOSE 8080 8081
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Ordering.API/Ordering.API.csproj", "Services/Ordering.API/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "Services/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Ordering.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
