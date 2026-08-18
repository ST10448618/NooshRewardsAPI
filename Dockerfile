# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
# Make sure the filename matches your actual .csproj exactly
COPY ["NooshRewardsApi.csproj", "./"]
RUN dotnet restore "NooshRewardsApi.csproj"

# Copy the rest of the code and publish
COPY . .
RUN dotnet publish "NooshRewardsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Setup the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render dynamically assigns a port via the PORT environment variable.
# This tells ASP.NET Core to listen on whatever port Render provides, defaulting to 8080.
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "NooshRewardsApi.dll"]