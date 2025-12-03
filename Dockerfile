FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything from root
COPY . .

# Restore using the solution file
RUN dotnet restore StorefrontAPI.sln

# Build the entire solution
RUN dotnet build StorefrontAPI.sln -c Release -o /app/build

# Publish the API project specifically
RUN dotnet publish src/Storefront.API/Storefront.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Render provides PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE ${PORT:-10000}

ENTRYPOINT ["dotnet", "Storefront.API.dll"]