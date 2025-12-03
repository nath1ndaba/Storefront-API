FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY StorefrontAPI.sln ./

# Copy all project files
COPY src/ ./src/

# Restore dependencies
RUN dotnet restore

# Build
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish src/Storefront.API/Storefront.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE ${PORT:-10000}

ENTRYPOINT ["dotnet", "Storefront.API.dll"]
