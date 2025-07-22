Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CDN.Directory.UI/CDN.Directory.UI.csproj", "CDN.Directory.UI/"]
COPY ["CDN.Directory.Core/CDN.Directory.Core.csproj", "CDN.Directory.Core/"]
COPY ["CDN.Directory.Infrastructure/CDN.Directory.Infrastructure.csproj", "CDN.Directory.Infrastructure/"]

Restore NuGet packages
RUN dotnet restore "CDN.Directory.UI/CDN.Directory.UI.csproj"

Copy everything and build
COPY . .
WORKDIR "/src/CDN.Directory.UI"
RUN dotnet build "CDN.Directory.UI.csproj" -c Release -o /app/build

Publish app
FROM build AS publish
RUN dotnet publish "CDN.Directory.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CDN.Directory.UI.dll"]