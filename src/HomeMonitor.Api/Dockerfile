FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HomeMonitor.Api/HomeMonitor.Api.csproj", "HomeMonitor.Api/"]
RUN dotnet restore "HomeMonitor.Api/HomeMonitor.Api.csproj"
COPY . .
WORKDIR "/src/HomeMonitor.Api"
RUN dotnet build "HomeMonitor.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeMonitor.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeMonitor.Api.dll"]
