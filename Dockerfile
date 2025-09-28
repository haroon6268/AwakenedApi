# Use the official .NET 8 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["AwakenedApi.csproj", "./"]
RUN dotnet restore "AwakenedApi.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "AwakenedApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "AwakenedApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Create a non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Start the application
ENTRYPOINT ["dotnet", "AwakenedApi.dll"]
