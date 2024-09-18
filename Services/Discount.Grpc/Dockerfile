FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080 8081

ENV ASPNETCORE_URLS=https://+:8081;http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Discount.Grpc/Discount.Grpc.csproj", "Services/Discount.Grpc/"]
RUN dotnet restore "Services/Discount.Grpc/Discount.Grpc.csproj"
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj"
COPY ["BuildingBlocks/BuildingBlocks/", "BuildingBlocks/BuildingBlocks/"]
COPY . .
WORKDIR "/src/Services/Discount.Grpc"
RUN dotnet build "Discount.Grpc.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Discount.Grpc.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Grpc.dll"]
# CMD ["tail", "-f", "/dev/null"]
