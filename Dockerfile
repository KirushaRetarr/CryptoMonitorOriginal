# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY CryptoMonitorOriginal/*.csproj ./CryptoMonitorOriginal/
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "CryptoMonitor.dll"]
