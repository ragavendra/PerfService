FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore PerfRunner/*.csproj

# Copy everything else and build
# COPY . ./
RUN dotnet publish -c Release -o out PerfRunner/*.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
EXPOSE 5110
EXPOSE 5000
EXPOSE 5277
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS "http://0.0.0.0:5000"
COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "PerfRunner.dll", "--environment=Development"]
ENTRYPOINT ["dotnet", "PerfRunner.dll"]