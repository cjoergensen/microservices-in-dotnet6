#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MeterDataManagement/MeterDataManagement.Meter/MeterDataManagement.Meter.csproj", "MeterDataManagement/MeterDataManagement.Meter/"]
COPY ["Shared/Shared.Logging/Logging.csproj", "Shared/Shared.Logging/"]
COPY ["Shared/Shared.Telemetry/Telemetry.csproj", "Shared/Shared.Telemetry/"]
RUN dotnet restore "MeterDataManagement/MeterDataManagement.Meter/MeterDataManagement.Meter.csproj"
COPY . .
WORKDIR "/src/MeterDataManagement/MeterDataManagement.Meter"
RUN dotnet build "MeterDataManagement.Meter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeterDataManagement.Meter.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcmePowerSolutions.MeterDataManagement.Meter.dll"]