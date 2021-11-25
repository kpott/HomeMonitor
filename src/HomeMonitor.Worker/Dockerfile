FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HomeMonitor.Worker/HomeMonitor.Worker.csproj", "HomeMonitor.Worker/"]
COPY ["HomeMonitor.Contracts/HomeMonitor.Contracts.csproj", "HomeMonitor.Contracts/"]
RUN dotnet restore "HomeMonitor.Worker/HomeMonitor.Worker.csproj"
COPY . .
WORKDIR "/src/HomeMonitor.Worker"
RUN dotnet build "HomeMonitor.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeMonitor.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeMonitor.Worker.dll"]
