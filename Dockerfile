# ============================================================================
# Storefront API - Production Dockerfile for Render
# ============================================================================
# Multi-stage build for optimal image size and security

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["StorefrontAPI.sln", "./"]
COPY ["src/Storefront.Domain/Storefront.Domain.csproj", "src/Storefront.Domain/"]
COPY ["src/Storefront.Application/Storefront.Application.csproj", "src/Storefront.Application/"]
COPY ["src/Storefront.Infrastructure/Storefront.Infrastructure.csproj", "src/Storefront.Infrastructure/"]
COPY ["src/Storefront.API/Storefront.API.csproj", "src/Storefront.API/"]

# Restore dependencies
RUN dotnet restore "src/Storefront.API/Storefront.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/Storefront.API"
RUN dotnet build "Storefront.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Storefront.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && \
    mkdir -p /app/logs && \
    chown -R appuser:appuser /app

# Copy published application
COPY --from=publish /app/publish .

# Switch to non-root user
USER appuser

# Render provides PORT environment variable dynamically
# Bind to 0.0.0.0 to accept connections from Render's load balancer
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port (Render typically uses 10000)
EXPOSE ${PORT:-10000}

# Run the application
ENTRYPOINT ["dotnet", "Storefront.API.dll"]
