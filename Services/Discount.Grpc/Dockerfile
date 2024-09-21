FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install grpc_health_probe to use healthcheck
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://github.com/grpc-ecosystem/grpc-health-probe/releases/latest/download/grpc_health_probe-linux-amd64 -O /usr/local/bin/grpc_health_probe && \
    chmod +x /usr/local/bin/grpc_health_probe

USER app
WORKDIR /app
EXPOSE 6002 6062
ENV ASPNETCORE_URLS=https://+:6062;http://+:6002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Discount.Grpc/Discount.Grpc.csproj", "Services/Discount.Grpc/"]
RUN dotnet restore "Services/Discount.Grpc/Discount.Grpc.csproj"
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