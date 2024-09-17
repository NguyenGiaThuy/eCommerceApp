FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080 8081

ENV ASPNETCORE_URLS=https://+:8081;http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Basket.API/Basket.API.csproj", "Services/Basket.API/"]
RUN dotnet restore "Services/Basket.API/Basket.API.csproj"
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj"
COPY ["BuildingBlocks/BuildingBlocks/", "BuildingBlocks/BuildingBlocks/"]
COPY . .
WORKDIR "/src/Services/Basket.API"
RUN dotnet build "Basket.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Basket.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
# CMD ["tail", "-f", "/dev/null"]
