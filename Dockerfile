FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
# RUN dotnet restore PerfRunner/*.csproj

# Copy everything else and build
# COPY . ./
RUN dotnet publish -c Release -o out PerfRunner/*.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PerfRunner.dll"]