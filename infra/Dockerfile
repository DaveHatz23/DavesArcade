# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["DavesArcade.sln", "./"]

# Copy project files
COPY ["src/DavesArcade.Api/DavesArcade.Api.csproj", "src/DavesArcade.Api/"]
COPY ["src/DavesArcade.Application/DavesArcade.Application.csproj", "src/DavesArcade.Application/"]
COPY ["src/DavesArcade.Domain/DavesArcade.Domain.csproj", "src/DavesArcade.Domain/"]
COPY ["src/DavesArcade.Infrastructure/DavesArcade.Infrastructure.csproj", "src/DavesArcade.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/DavesArcade.Api"
RUN dotnet build "DavesArcade.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DavesArcade.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user for security
RUN addgroup --system --gid 1001 appuser && \
    adduser --system --uid 1001 --ingroup appuser appuser

# Copy published app
COPY --from=publish /app/publish .

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "DavesArcade.Api.dll"]
