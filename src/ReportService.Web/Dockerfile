FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReportService.Web/ReportService.Web.csproj", "ReportService.Web/"]
COPY ["ReportService.Core/ReportService.Core.csproj", "ReportService.Core/"]
COPY ["ReportService.Infrastructure/ReportService.Infrastructure.csproj", "ReportService.Infrastructure/"]
RUN dotnet restore "ReportService.Web/ReportService.Web.csproj"
COPY . .
WORKDIR "/src/ReportService.Web"
RUN dotnet build "ReportService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReportService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportService.Web.dll"]
